using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record DisputeAlternative(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type, // REFUND, BENEFIT ou ADDITIONAL_TIME
    [property: JsonPropertyName("metadata")] DisputeAlternativeMetadata Group
);