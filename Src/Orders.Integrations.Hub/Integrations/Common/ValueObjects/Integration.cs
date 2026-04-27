namespace Orders.Integrations.Hub.Integrations.Common.ValueObjects;

public record Integration(
    string? TenantId,
    string MerchantId,
    string ClientId,
    string ClientSecret,
    bool AutoAccept,
    IntegrationMode Mode
);