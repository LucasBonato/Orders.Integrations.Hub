using System.Collections.Specialized;
using System.Net;

using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public sealed class WireMockApi {
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