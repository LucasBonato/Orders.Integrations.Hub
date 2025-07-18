﻿using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record HandshakeDispute(
    [property: JsonPropertyName("disputeId")] string DisputeId,
    [property: JsonPropertyName("parentDisputeId")] string? ParentDisputeId,
    [property: JsonPropertyName("action")] [property: JsonConverter(typeof(JsonStringEnumConverter))] HandshakeAction Action,
    [property: JsonPropertyName("message")] string? Message,
    [property: JsonPropertyName("alternatives")] List<DisputeAlternative>? Alternatives,
    [property: JsonPropertyName("expiresAt")] DateTime ExpiresAt,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("handshakeType")] [property: JsonConverter(typeof(JsonStringEnumConverter))] HandshakeType HandshakeType,
    [property: JsonPropertyName("handshakeGroup")] [property: JsonConverter(typeof(JsonStringEnumConverter))] HandshakeGroup HandshakeGroup,
    [property: JsonPropertyName("timeoutAction")] [property: JsonConverter(typeof(JsonStringEnumConverter))] TimeoutAction TimeoutAction,
    [property: JsonPropertyName("metadata")] HandshakeMetadata? Metadata
);