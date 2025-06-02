using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record SelectedDisputeAlternative(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] HandshakeAlternativeType Type,
    [property: JsonPropertyName("metadata")] DisputeAlternativeMetadata Metadata
);