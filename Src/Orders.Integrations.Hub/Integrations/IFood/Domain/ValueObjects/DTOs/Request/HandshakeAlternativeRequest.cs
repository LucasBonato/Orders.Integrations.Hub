using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

public record HandshakeAlternativeRequest(
    HandshakeAlternativeType Type,
    HandshakeAlternativeMetadata Metadata
);