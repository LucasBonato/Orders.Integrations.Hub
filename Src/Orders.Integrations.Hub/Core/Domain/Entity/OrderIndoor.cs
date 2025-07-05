using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderIndoor(
    [property: JsonPropertyName("mode")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderIndoorMode Mode,
    [property: JsonPropertyName("indoorDateTime")] DateTime IndoorDateTime,
    [property: JsonPropertyName("place")] string? Place,
    [property: JsonPropertyName("seat")] string? Seat,
    [property: JsonPropertyName("tab")] string? Tab
);