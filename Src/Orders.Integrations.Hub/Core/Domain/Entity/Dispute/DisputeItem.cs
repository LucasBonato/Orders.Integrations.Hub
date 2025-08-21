namespace Orders.Integrations.Hub.Core.Domain.Entity.Dispute;

public record DisputeItem(
    string ExternalId,
    string? ExternalUniqueId,
    string Sku,
    int Index,
    int Quantity,
    Price Price,
    string ReasonMessage
);