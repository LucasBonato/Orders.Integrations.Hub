using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Internal;

namespace Orders.Integrations.Hub.Integrations.Common.Contracts;

/// <summary>
/// This is used to maintain the context of the integration for the request for multi-tenant level
/// </summary>
public interface IIntegrationContext {
    string? MerchantId { get; set; }
    IntegrationResponse? Integration { get; set; }
}