using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Extensions;

public static class IntegrationResolveExtension
{
    public static RappiIntegrationResolved Resolve(this BizPikIntegrationResponse integration)
    {
        string clientId = integration.Settings.FirstOrDefault(x => x.Name is "rappi_client_id")?.Value ?? throw new("Rappi client id not found");
        string clientSecret = integration.Settings.FirstOrDefault(x => x.Name is "rappi_client_secret")?.Value ?? throw new("Rappi client secret not found"); ;
        string storeId = integration.Settings.FirstOrDefault(x => x.Name is "store_id")?.Value ?? throw new("Rappi store id not found"); ;

        return new RappiIntegrationResolved(
            RappiClientId: clientId,
            RappiStoreId: storeId,
            RappiClientSecret: clientSecret
        );
    }
}