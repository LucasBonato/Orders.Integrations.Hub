namespace Orders.Integrations.Hub.Integrations.Common;

public record IntegrationResolved(
    string? TenantId,
    string MerchantId,
    string ClientId,
    string ClientSecret,
    bool AutoAccept
);