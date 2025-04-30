using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity.Address;

public record AddressCoordinates(
    [property: JsonPropertyName("latitude")] decimal Latitude,
    [property: JsonPropertyName("longitude")] decimal Longitude
);