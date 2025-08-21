namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record GarnishItem(
    string Id,
    string ParentUniqueId,
    string ExternalCode,
    int Quantity,
    int Index,
    Amount Amount,
    string? Reason
);