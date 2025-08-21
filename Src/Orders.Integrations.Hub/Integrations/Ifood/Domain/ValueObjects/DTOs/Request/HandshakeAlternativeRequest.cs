using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record HandshakeAlternativeRequest(
    HandshakeAlternativeType Type,
    HandshakeAlternativeMetadata Metadata
);