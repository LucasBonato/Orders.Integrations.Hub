using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record HandshakeDispute(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("parentDisputeId")] string? ParentDisputeId,
    [property: JsonPropertyName("action")] HandshakeAction Action,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("alternatives")] List<DisputeAlternative>? Alternatives,
    [property: JsonPropertyName("expiresAt")] DateTime ExpiresAt,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("handshakeType")] HandshakeType HandshakeType,
    [property: JsonPropertyName("handshakeGroup")] HandshakeGroup HandshakeGroup,
    [property: JsonPropertyName("timeoutAction")] TimeoutAction TimeoutAction,
    [property: JsonPropertyName("metadata")] HandshakeMetadata? Metadata
);