using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;

namespace Orders.Integrations.Hub.Integrations.Common.Application;

/// <inheritdoc/>
public class IntegrationContext : IIntegrationContext {
    public string? TenantId { get; set; }
    public string? MerchantId { get; set; }
    public Integration? Integration { get; set; }
}