using Orders.Integrations.Hub.Integrations.Common.Contracts;

namespace Orders.Integrations.Hub.Integrations.Common.Application;

/// <inheritdoc/>
public class IntegrationContext : IIntegrationContext {
    public string? TenantId { get; set; }
    public string? MerchantId { get; set; }
    public IntegrationResolved? Integration { get; set; }
}