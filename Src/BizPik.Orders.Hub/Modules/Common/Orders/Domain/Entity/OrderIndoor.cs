using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Domain.Entity;

public record OrderIndoor(
    [property: JsonPropertyName("mode")] string Mode,
    [property: JsonPropertyName("indoorDateTime")] DateTime IndoorDateTime,
    [property: JsonPropertyName("place")] string? Place,
    [property: JsonPropertyName("seat")] string? Seat,
    [property: JsonPropertyName("tab")] string? Tab
);