namespace Orders.Integrations.Hub.Integrations.Common.Contracts;

/// <summary>
/// This is used to maintain the context of the integration for the request for multi-tenant level
/// </summary>
public interface IIntegrationContext {
    string? TenantId { get; set; }
    string? MerchantId { get; set; }
    IntegrationResolved? Integration { get; set; }
}