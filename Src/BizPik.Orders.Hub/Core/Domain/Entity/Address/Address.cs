using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.Entity.Address;

public record Address(
    [property: JsonPropertyName("country")] string Country,
    [property: JsonPropertyName("state")] string State,
    [property: JsonPropertyName("city")] string City,
    [property: JsonPropertyName("district")] string District,
    [property: JsonPropertyName("street")] string Street,
    [property: JsonPropertyName("number")] string Number,
    [property: JsonPropertyName("complement")] string Complement,
    [property: JsonPropertyName("reference")] string Reference,
    [property: JsonPropertyName("formattedAddress")] string FormattedAddress,
    [property: JsonPropertyName("postalCode")] string PostalCode,
    [property: JsonPropertyName("coordinates")] AddressCoordinates Coordinates
);