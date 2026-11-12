using Orders.Integrations.Hub.IntegrationTests.Payloads;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public sealed class Food99ApiMock(WireMockAdminClient adminClient) : WireMockApiMock(adminClient) {
    private const string Integration = "Food99";

    private readonly string _success = PayloadLoader.Load(
        Integration,
        "success-response"
    );

    public Task StubTokenAsync(string token = "food99-token", CancellationToken cancellationToken = default)
        => RespondAsync(
            "/v1/auth/authtoken/get",
            HttpMethod.Get,
            body: PayloadLoader.Load(
                Integration,
                "auth",
                ("accessToken", token)
            ), cancellationToken: cancellationToken
        );

    public Task StubCommandEndpointsAsync(CancellationToken cancellationToken = default)
    {
        return Task.WhenAll(
            RespondAsync("/v1/order/order/confirm", HttpMethod.Post, body: _success, cancellationToken: cancellationToken),
            RespondAsync("/v1/order/order/ready", HttpMethod.Post, body: _success, cancellationToken: cancellationToken),
            RespondAsync("/v1/order/order/delivered", HttpMethod.Post, body: _success, cancellationToken: cancellationToken),
            RespondAsync("/v1/order/order/cancel", HttpMethod.Post, body: _success, cancellationToken: cancellationToken)
        );
    }
}
