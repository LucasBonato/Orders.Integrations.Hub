using System.Collections.Specialized;
using System.Net;

using Orders.Integrations.Hub.IntegrationTests.Payloads;

using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Mocks;

public static class WireMockApi
{
    public static void Respond(
        WireMockServer server,
        string path,
        HttpMethod httpMethod,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string body = "{}",
        string contentType = "application/json"
    ) {
        server
            .Given(
                Request.Create()
                    .WithPath(new WildcardMatcher(path))
                    .UsingMethod(httpMethod.Method)
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode((int)statusCode)
                    .WithHeader("Content-Type", contentType)
                    .WithBody(body)
            );
    }

    public static async Task WaitForRequestAsync(
        WireMockServer server,
        string path,
        CancellationToken cancellationToken,
        TimeSpan? timeout = null
    ) {
        using CancellationTokenSource timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutSource.CancelAfter(timeout ?? TimeSpan.FromSeconds(10));

        TaskCompletionSource requestReceived = new(TaskCreationOptions.RunContinuationsAsynchronously);
        NotifyCollectionChangedEventHandler handler = (_, _) => {
            if (HasRequest(server, path))
                requestReceived.TrySetResult();
        };

        server.LogEntriesChanged += handler;
        try {
            if (!HasRequest(server, path))
                await requestReceived.Task.WaitAsync(timeoutSource.Token);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested) {
            string actualPaths = string.Join(
                ", ", 
                server.LogEntries
                    .Select(entry => entry.RequestMessage?.Path)
                    .Where(requestPath => requestPath is not null)
            );
            throw new TimeoutException($"Timed out waiting for '{path}'. Recorded request paths: {actualPaths}");
        }
        finally {
            server.LogEntriesChanged -= handler;
        }

        return;

        static bool HasRequest(WireMockServer server, string path)
            => server.LogEntries.Any(entry => string.Equals(entry.RequestMessage?.Path, path, StringComparison.OrdinalIgnoreCase));
    }

    public static int RequestCount(WireMockServer server, string path)
        => server.LogEntries.Count(entry => string.Equals(entry.RequestMessage?.Path, path, StringComparison.OrdinalIgnoreCase));

    public static string LastRequestBody(WireMockServer server, string path)
        => server.LogEntries
            .Last(entry => string.Equals(entry.RequestMessage?.Path, path, StringComparison.OrdinalIgnoreCase))
            .RequestMessage?.Body
                ?? throw new InvalidOperationException($"The request to '{path}' had no body.");
}

public sealed class OrdersApiMock(WireMockServer server) {
    public void StubCreateOrder(HttpStatusCode statusCode = HttpStatusCode.OK)
        => WireMockApi.Respond(server, "/Orders", HttpMethod.Post, statusCode);

    public void StubPatchOrder(HttpStatusCode statusCode = HttpStatusCode.OK)
        => WireMockApi.Respond(server, "/Orders", HttpMethod.Patch, statusCode);

    public void StubPatchOrderDispute(HttpStatusCode statusCode = HttpStatusCode.OK)
        => WireMockApi.Respond(server, "/Orders/dispute", HttpMethod.Patch, statusCode);

    public Task WaitForCreateOrderAsync(CancellationToken cancellationToken)
        => WireMockApi.WaitForRequestAsync(server, "/Orders", cancellationToken);

    public Task WaitForPatchOrderAsync(CancellationToken cancellationToken)
        => WireMockApi.WaitForRequestAsync(server, "/Orders", cancellationToken);

    public Task WaitForPatchOrderDisputeAsync(CancellationToken cancellationToken)
        => WireMockApi.WaitForRequestAsync(server, "/Orders/dispute", cancellationToken);

    public int RequestCount(string path) => WireMockApi.RequestCount(server, path);
    public string LastRequestBody(string path) => WireMockApi.LastRequestBody(server, path);
}

public sealed class IFoodApiMock(WireMockServer server) {
    private const string Integration = "IFood";
    
    public void StubToken(string token = "ifood-token")
        => WireMockApi.Respond(
            server,
            "/authentication/v1.0/oauth/token",
            HttpMethod.Post,
            body: PayloadLoader.Load(
                Integration,
                "auth",
                ("accessToken", token)
            )
        );

    public void StubOrderDetails(string orderId = "order-1")
        => WireMockApi.Respond(
            server,
            "/order/v1.0/orders/*",
            HttpMethod.Get,
            body: PayloadLoader.Load(
                Integration,
                "order-details",
                ("orderId", orderId),
                ("createdAt", DateTime.UtcNow.ToString("O")),
                ("preparationStartDateTime", DateTime.UtcNow.ToString("O")),
                ("deliveryDateTime", DateTime.UtcNow.AddHours(1).ToString("O"))
            )
        );

    public void StubCancellationReasons()
        => WireMockApi.Respond(
            server,
            "/order/v1.0/orders/*/cancellationReasons",
            HttpMethod.Get,
            body: "[{\"cancelCodeId\":\"0\",\"description\":\"Customer request\"}]"
        );

    public void StubCommandEndpoints() {
        WireMockApi.Respond(server, "/order/v1.0/orders/*/confirm", HttpMethod.Post);
        WireMockApi.Respond(server, "/order/v1.0/orders/*/preparationStarted", HttpMethod.Post);
        WireMockApi.Respond(server, "/order/v1.0/orders/*/readyToPickup", HttpMethod.Post);
        WireMockApi.Respond(server, "/order/v1.0/orders/*/dispatch", HttpMethod.Post);
        WireMockApi.Respond(server, "/order/v1.0/orders/*/requestCancellation", HttpMethod.Post);
        WireMockApi.Respond(server, "/order/v1.0/disputes/*/accept", HttpMethod.Post);
        WireMockApi.Respond(server, "/order/v1.0/disputes/*/reject", HttpMethod.Post);
        WireMockApi.Respond(server, "/order/v1.0/disputes/*/alternatives/*", HttpMethod.Post);
    }

    public void StubEvidence(string path = "/evidence-1.png")
        => WireMockApi.Respond(
            server,
            path,
            HttpMethod.Get,
            body: "test-image",
            contentType: "image/png"
        );
}

public sealed class RappiApiMock(WireMockServer server)
{
    private const string Integration = "Rappi";
    
    public void StubToken(string token = "rappi-token")
        => WireMockApi.Respond(
            server,
            "/oauth/token",
            HttpMethod.Post,
            body: PayloadLoader.Load(
                Integration,
                "auth",
                ("accessToken", token)
            )
        );

    public void StubCommandEndpoints() {
        WireMockApi.Respond(server, "/restaurants/orders/*/orders/*/take", HttpMethod.Put);
        WireMockApi.Respond(server, "/orders/*/take/*", HttpMethod.Put);
        WireMockApi.Respond(server, "/orders/*/reject", HttpMethod.Put);
        WireMockApi.Respond(server, "/orders/*/ready-for-pickup", HttpMethod.Put);
    }
}

public sealed class Food99ApiMock(WireMockServer server) {
    private const string Integration = "Food99";
    
    private readonly string _success = PayloadLoader.Load(
        Integration,
        "success-response"
    );

    public void StubToken(string token = "food99-token")
        => WireMockApi.Respond(
            server,
            "/v1/auth/authtoken/get", 
            HttpMethod.Get,
            body: PayloadLoader.Load(
                Integration,
                "auth",
                ("accessToken", token)
            )
        );

    public void StubCommandEndpoints() {
        WireMockApi.Respond(server, "/v1/order/order/confirm", HttpMethod.Post, body: _success);
        WireMockApi.Respond(server, "/v1/order/order/ready", HttpMethod.Post, body: _success);
        WireMockApi.Respond(server, "/v1/order/order/delivered", HttpMethod.Post, body: _success);
        WireMockApi.Respond(server, "/v1/order/order/cancel", HttpMethod.Post, body: _success);
    }
}
