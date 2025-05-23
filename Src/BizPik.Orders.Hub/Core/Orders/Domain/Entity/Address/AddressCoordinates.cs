using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Entity.Address;

public record AddressCoordinates(
    [property: JsonPropertyName("latitude")] decimal Latitude,
    [property: JsonPropertyName("longitude")] decimal Longitude
);