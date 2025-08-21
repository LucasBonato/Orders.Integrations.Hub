using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record SelectedDisputeAlternative(
    string Id,
    HandshakeAlternativeType Type,
    IfoodDisputeAlternativeMetadata Metadata
);