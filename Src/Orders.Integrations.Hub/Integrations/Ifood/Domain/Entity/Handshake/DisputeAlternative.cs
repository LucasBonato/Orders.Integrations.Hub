using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record DisputeAlternative(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] HandshakeAlternativeType Type,
    [property: JsonPropertyName("metadata")] DisputeAlternativeMetadata Metadata
);