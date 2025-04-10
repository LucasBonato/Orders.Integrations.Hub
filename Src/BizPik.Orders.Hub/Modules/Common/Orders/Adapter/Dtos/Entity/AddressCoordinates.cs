using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity;

public record AddressCoordinates(
    [property: JsonPropertyName("latitude")] decimal Latitude,
    [property: JsonPropertyName("longitude")] decimal Longitude
);