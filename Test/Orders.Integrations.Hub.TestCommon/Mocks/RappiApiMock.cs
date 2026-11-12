using Orders.Integrations.Hub.IntegrationTests.Payloads;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public sealed class RappiApiMock(
    WireMockAdminClient adminClient
) : WireMockApiMock(adminClient) {
    private const string Integration = "Rappi";

    public Task StubTokenAsync(string token = "rappi-token", CancellationToken cancellationToken = default)
        => RespondAsync(
            "/oauth/token",
            HttpMethod.Post,
            body: PayloadLoader.Load(
                Integration,
                "auth",
                ("accessToken", token)
            ), cancellationToken: cancellationToken
        );

    public Task StubCommandEndpointsAsync(CancellationToken cancellationToken = default) {
        return Task.WhenAll(
            RespondAsync("/restaurants/orders/*/orders/*/take", HttpMethod.Put, cancellationToken: cancellationToken),
            RespondAsync("/orders/*/take/*", HttpMethod.Put, cancellationToken: cancellationToken),
            RespondAsync("/orders/*/reject", HttpMethod.Put, cancellationToken: cancellationToken),
            RespondAsync("/orders/*/ready-for-pickup", HttpMethod.Put, cancellationToken: cancellationToken)
        );
    }

}
