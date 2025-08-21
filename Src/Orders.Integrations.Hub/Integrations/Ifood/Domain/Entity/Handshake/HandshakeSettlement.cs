using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record HandshakeSettlement(
    string DisputeId,
    Status Status,
    string? Reason,
    SelectedDisputeAlternative SelectedDisputeAlternative,
    DateTime CreatedAt
);