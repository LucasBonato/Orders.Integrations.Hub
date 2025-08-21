using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Internal;
using Orders.Integrations.Hub.Integrations.Common.Contracts;

namespace Orders.Integrations.Hub.Integrations.Common.Application;

/// <inheritdoc/>
public class IntegrationContext : IIntegrationContext {
    public string? MerchantId { get; set; }
    public IntegrationResponse? Integration { get; set; }
}