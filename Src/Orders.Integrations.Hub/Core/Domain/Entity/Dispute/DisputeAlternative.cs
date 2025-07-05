using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Dispute;

public record DisputeAlternative(
    [property: JsonPropertyName("alternativeId")] string AlternativeId,
    [property: JsonPropertyName("type")] HandshakeAlternativeType Type,
    [property: JsonPropertyName("price")] Price? Price,
    [property: JsonPropertyName("allowedTimesInMinutes")] List<int>? AllowedTimesInMinutes,
    [property: JsonPropertyName("allowedTimesReasons")] List<string>? AllowedTimesReasons
);