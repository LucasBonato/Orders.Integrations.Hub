namespace Orders.Integrations.Hub.Integrations.Common.ValueObjects;

public record IntegrationResolved(
    string? TenantId,
    string MerchantId,
    string ClientId,
    string ClientSecret,
    bool AutoAccept
);