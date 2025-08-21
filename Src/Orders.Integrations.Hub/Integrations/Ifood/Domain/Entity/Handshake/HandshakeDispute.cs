using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record HandshakeDispute(
    string DisputeId,
    string? ParentDisputeId,
    HandshakeAction Action,
    string? Message,
    List<IfoodDisputeAlternative>? Alternatives,
    DateTime ExpiresAt,
    DateTime CreatedAt,
    HandshakeType HandshakeType,
    HandshakeGroup HandshakeGroup,
    TimeoutAction TimeoutAction,
    HandshakeMetadata? Metadata
);