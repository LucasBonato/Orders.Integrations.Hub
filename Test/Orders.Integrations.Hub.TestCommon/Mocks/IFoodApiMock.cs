using Orders.Integrations.Hub.IntegrationTests.Payloads;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public sealed class IFoodApiMock(WireMockAdminClient adminClient) : WireMockApiMock(adminClient) {
    private const string Integration = "IFood";

    public Task StubTokenAsync(string token = "ifood-token", CancellationToken cancellationToken = default)
        => RespondAsync(
            "/authentication/v1.0/oauth/token",
            HttpMethod.Post,
            body: PayloadLoader.Load(
                Integration,
                "auth",
                ("accessToken", token)
            ), cancellationToken: cancellationToken
        );

    public Task StubOrderDetailsAsync(string orderId = "order-1", CancellationToken cancellationToken = default)
        => RespondAsync(
            "/order/v1.0/orders/*",
            HttpMethod.Get,
            body: PayloadLoader.Load(
                Integration,
                "order-details",
                ("orderId", orderId),
                ("createdAt", DateTime.UtcNow.ToString("O")),
                ("preparationStartDateTime", DateTime.UtcNow.ToString("O")),
                ("deliveryDateTime", DateTime.UtcNow.AddHours(1).ToString("O"))
            ), cancellationToken: cancellationToken
        );

    public Task StubCancellationReasonsAsync(CancellationToken cancellationToken = default)
        => RespondAsync(
            "/order/v1.0/orders/*/cancellationReasons",
            HttpMethod.Get,
            body: "[{\"cancelCodeId\":\"0\",\"description\":\"Customer request\"}]",
            cancellationToken: cancellationToken
        );  

    public async Task StubCommandEndpointsAsync(CancellationToken cancellationToken = default)
    {
        await Task.WhenAll(
            RespondAsync("/order/v1.0/orders/*/confirm", HttpMethod.Post, cancellationToken: cancellationToken),
            RespondAsync("/order/v1.0/orders/*/preparationStarted", HttpMethod.Post, cancellationToken: cancellationToken),
            RespondAsync("/order/v1.0/orders/*/readyToPickup", HttpMethod.Post, cancellationToken: cancellationToken),
            RespondAsync("/order/v1.0/orders/*/dispatch", HttpMethod.Post, cancellationToken: cancellationToken),
            RespondAsync("/order/v1.0/orders/*/requestCancellation", HttpMethod.Post, cancellationToken: cancellationToken),
            RespondAsync("/order/v1.0/disputes/*/accept", HttpMethod.Post, cancellationToken: cancellationToken),
            RespondAsync("/order/v1.0/disputes/*/reject", HttpMethod.Post, cancellationToken: cancellationToken),
            RespondAsync("/order/v1.0/disputes/*/alternatives/*", HttpMethod.Post, cancellationToken: cancellationToken)
        );
    }

    public Task StubEvidenceAsync(string path = "/evidence-1.png", CancellationToken cancellationToken = default)
        => RespondAsync(
            path,
            HttpMethod.Get,
            body: "test-image",
            contentType: "image/png",
            cancellationToken: cancellationToken
        );
}
