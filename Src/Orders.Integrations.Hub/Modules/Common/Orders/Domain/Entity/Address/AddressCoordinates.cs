using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Domain.Entity.Address;

public record AddressCoordinates(
    [property: JsonPropertyName("latitude")] decimal Latitude,
    [property: JsonPropertyName("longitude")] decimal Longitude
);