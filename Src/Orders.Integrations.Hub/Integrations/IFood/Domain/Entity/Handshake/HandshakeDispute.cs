using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;

public record HandshakeDispute(
    string DisputeId,
    string? ParentDisputeId,
    HandshakeAction Action,
    string? Message,
    List<IFoodDisputeAlternative>? Alternatives,
    DateTime ExpiresAt,
    DateTime CreatedAt,
    HandshakeType HandshakeType,
    HandshakeGroup HandshakeGroup,
    TimeoutAction TimeoutAction,
    HandshakeMetadata? Metadata
);