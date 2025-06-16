using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.BizPik;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

namespace BizPik.Orders.Hub.Integrations.Rappi.Application.Extensions;

public static class IntegrationResolveExtension
{
    public static RappiIntegrationResolved Resolve(this BizPikIntegrationResponse integration)
    {
        string clientId = integration.Settings.FirstOrDefault(integrationSettings => integrationSettings.Name is "rappi_client_id")?.Value ?? throw new Exception("Rappi client id not found");
        string clientSecret = integration.Settings.FirstOrDefault(integrationSettings => integrationSettings.Name is "rappi_client_secret")?.Value ?? throw new Exception("Rappi client secret not found");
        string storeId = integration.Settings.FirstOrDefault(integrationSettings => integrationSettings.Name is "store_id")?.Value ?? throw new Exception("Rappi store id not found"); ;

        return new RappiIntegrationResolved(
            RappiClientId: clientId,
            RappiStoreId: storeId,
            RappiClientSecret: clientSecret
        );
    }
}