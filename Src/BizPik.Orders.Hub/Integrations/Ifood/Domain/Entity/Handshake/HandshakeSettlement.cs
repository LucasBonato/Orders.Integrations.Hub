using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record HandshakeSettlement(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("disputeId")] string DisputeId,
    [property: JsonPropertyName("status")] Status Status,
    [property: JsonPropertyName("reason")] string? Reason,
    [property: JsonPropertyName("selectedDisputeAlternative")] SelectedDisputeAlternative SelectedDisputeAlternative,
    [property: JsonPropertyName("expiresAt")] DateTime ExpiresAt,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("handshakeType")] HandshakeType HandshakeType,
    [property: JsonPropertyName("handshakeGroup")] HandshakeGroup HandshakeGroup,
    [property: JsonPropertyName("timeoutAction")] TimeoutAction TimeoutAction,
    [property: JsonPropertyName("metadata")] List<HandshakeMetadata>? Metadata
);