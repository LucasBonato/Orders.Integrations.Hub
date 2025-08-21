namespace Orders.Integrations.Hub.Core.Domain.Entity.Dispute;

public record DisputeItemOption(
    string ExternalId,
    string? ParentExternalUniqueId,
    string Sku,
    int Index,
    int Quantity,
    Price Price,
    string ReasonMessage
);