using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Delivery;

public record Coordinates(
    [property: JsonPropertyName("latitude")] decimal Latitude,
    [property: JsonPropertyName("longitude")] decimal Longitude
);