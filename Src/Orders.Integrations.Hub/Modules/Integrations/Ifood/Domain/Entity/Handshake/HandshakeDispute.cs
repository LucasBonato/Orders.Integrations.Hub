using System.Text.Json.Serialization;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;
using Action = Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake.Action;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Handshake;

public record HandshakeDispute(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("parentDisputeId")] string? ParentDisputeId,
    [property: JsonPropertyName("action")] Action Action,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("alternatives")] List<DisputeAlternative>? Alternatives,
    [property: JsonPropertyName("expiresAt")] DateTime ExpiresAt,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("handshakeType")] HandshakeType HandshakeType,
    [property: JsonPropertyName("handshakeGroup")] HandshakeGroup HandshakeGroup,
    [property: JsonPropertyName("timeoutAction")] TimeoutAction TimeoutAction,
    [property: JsonPropertyName("metadata")] List<HandshakeMetadata>? Metadata
);