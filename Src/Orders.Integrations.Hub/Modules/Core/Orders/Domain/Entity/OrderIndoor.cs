using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity;

public record OrderIndoor(
    [property: JsonPropertyName("mode")] OrderIndoorMode Mode,
    [property: JsonPropertyName("indoorDateTime")] DateTime IndoorDateTime,
    [property: JsonPropertyName("place")] string? Place,
    [property: JsonPropertyName("seat")] string? Seat,
    [property: JsonPropertyName("tab")] string? Tab
);