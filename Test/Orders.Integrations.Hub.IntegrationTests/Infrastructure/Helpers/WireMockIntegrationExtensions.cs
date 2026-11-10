using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.TestCommon.Contracts;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Helpers;

public static class WireMockIntegrationExtensions
{
    public static async Task StubIntegrationAsync(
        this WireMockServers servers,
        IIntegrationContract contract,
        CancellationToken cancellationToken = default
    ) {
        switch (contract.Descriptor.Key.ToLowerInvariant())
        {
            case "ifood":
                await Task.WhenAll(
                    servers.IFoodApi.StubTokenAsync(cancellationToken: cancellationToken),
                    servers.IFoodApi.StubCommandEndpointsAsync(cancellationToken)
                );
                break;
            case "rappi":
                await Task.WhenAll(
                    servers.RappiApi.StubTokenAsync(cancellationToken: cancellationToken),
                    servers.RappiApi.StubCommandEndpointsAsync(cancellationToken)
                );
                break;
            case "food99":
                await Task.WhenAll(
                    servers.Food99Api.StubTokenAsync(cancellationToken: cancellationToken),
                    servers.Food99Api.StubCommandEndpointsAsync(cancellationToken)
                );
                break;
        }
    }
}
