using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;

namespace Orders.Integrations.Hub.TestCommon.Fixtures;

public static class ObjectMother
{
    public static Integration CreateIntegration(
        string? merchantId = null,
        string? clientId = null,
        string? clientSecret = null,
        bool autoAccept = false,
        IntegrationMode mode = IntegrationMode.Distributed
    ) => new(
        TenantId: "tenant-1",
        MerchantId: merchantId ?? "merchant-1",
        ClientId: clientId ?? "client-id",
        ClientSecret: clientSecret ?? "test-secret",
        AutoAccept: autoAccept,
        Mode: mode
    );

    public static IIntegrationContext CreateIntegrationContext(
        string? tenantId = null,
        string? merchantId = null,
        Integration? integration = null
    )
    {
        var context = new IntegrationContext
        {
            TenantId = tenantId ?? "tenant-1",
            MerchantId = merchantId ?? "merchant-1",
            Integration = integration ?? CreateIntegration()
        };
        return context;
    }
}

public class IntegrationContext : IIntegrationContext
{
    public string? TenantId { get; set; }
    public string? MerchantId { get; set; }
    public Integration? Integration { get; set; }
}