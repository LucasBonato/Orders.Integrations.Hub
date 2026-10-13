using Orders.Integrations.Hub.TestCommon.Contracts;
using Orders.Integrations.Hub.TestCommon.Host;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Helpers;

public static class WireMockIntegrationExtensions
{
    public static void StubIntegration(this WireMockServers servers, IIntegrationContract contract)
    {
        switch (contract.Descriptor.Key.ToLowerInvariant())
        {
            case "ifood":
                servers.IFoodApi.StubToken();
                servers.IFoodApi.StubCommandEndpoints();
                break;
            case "rappi":
                servers.RappiApi.StubToken();
                servers.RappiApi.StubCommandEndpoints();
                break;
            case "food99":
                servers.Food99Api.StubToken();
                servers.Food99Api.StubCommandEndpoints();
                break;
        }
    }
}
