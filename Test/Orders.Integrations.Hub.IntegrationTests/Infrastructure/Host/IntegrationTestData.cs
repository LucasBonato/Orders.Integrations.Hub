using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;
using Orders.Integrations.Hub.IntegrationTests.Contracts;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;

public static class IntegrationTestData
{
    public static IntegrationContext GetIntegrationContext(IIntegrationContract? contract = null) {
        string integration = contract is not null 
            ? contract.Descriptor.Key.ToLowerInvariant() + "-"
            : string.Empty;
        return new IntegrationContext {
            MerchantId = $"{integration}merchant-id",
            Integration = new Integration(
                TenantId: $"{integration}tenant-1",
                MerchantId: $"{integration}merchant-id",
                ClientId: $"{integration}client-id",
                ClientSecret: $"{integration}client-secret",
                AutoAccept: false,
                Mode: IntegrationMode.Centralized
            )
        };
    }
}