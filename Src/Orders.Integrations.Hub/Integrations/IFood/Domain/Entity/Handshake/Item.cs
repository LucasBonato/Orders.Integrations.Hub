namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;

public record Item(
    string Id,
    string UniqueId,
    string? ExternalCode,
    string? IntegrationId,
    int Quantity,
    int Index,
    Amount Amount,
    string? Reason
);