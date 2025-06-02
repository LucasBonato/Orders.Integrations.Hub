using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Entity.Dispute;

public record DisputeAlternative(
    [property: JsonPropertyName("alternativeId")] string AlternativeId,
    [property: JsonPropertyName("type")] HandshakeAlternativeType Type,
    [property: JsonPropertyName("price")] Price? Price,
    [property: JsonPropertyName("allowedTimesInMinutes")] List<int>? AllowedTimesInMinutes,
    [property: JsonPropertyName("allowedTimesReasons")] List<string>? AllowedTimesReasons
);