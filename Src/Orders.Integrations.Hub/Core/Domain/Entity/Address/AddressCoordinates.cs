using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Address;

public record AddressCoordinates(
    [property: JsonPropertyName("latitude")] decimal Latitude,
    [property: JsonPropertyName("longitude")] decimal Longitude
);