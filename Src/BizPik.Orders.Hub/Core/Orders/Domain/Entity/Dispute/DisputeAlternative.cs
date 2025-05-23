using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Entity.Dispute;

public record DisputeAlternative(
    [property: JsonPropertyName("alternativeId")] string AlternativeId,
    [property: JsonPropertyName("type")] string Type, // Should be an ENUM REFUND, DISCOUNT, ADDITIONAL_TIME
    [property: JsonPropertyName("price")] Price? Price,
    [property: JsonPropertyName("allowedTimesInMinutes")] List<int>? AllowedTimesInMinutes,
    [property: JsonPropertyName("allowedTimesReasons")] List<string>? AllowedTimesReasons
);