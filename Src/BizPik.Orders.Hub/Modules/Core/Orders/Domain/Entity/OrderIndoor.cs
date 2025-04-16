using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;

public record OrderIndoor(
    [property: JsonPropertyName("mode")] OrderIndoorMode Mode,
    [property: JsonPropertyName("indoorDateTime")] DateTime IndoorDateTime,
    [property: JsonPropertyName("place")] string? Place,
    [property: JsonPropertyName("seat")] string? Seat,
    [property: JsonPropertyName("tab")] string? Tab
);