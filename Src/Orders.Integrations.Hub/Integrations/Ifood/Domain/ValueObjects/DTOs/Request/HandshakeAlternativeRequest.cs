using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record HandshakeAlternativeRequest(
    [property: JsonPropertyName("type")] [property: JsonConverter(typeof(JsonStringEnumConverter))] HandshakeAlternativeType Type,
    [property: JsonPropertyName("metadata")] HandshakeAlternativeMetadata Metadata
);