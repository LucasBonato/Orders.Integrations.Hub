using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order.Delivery;

public record DeliveryAddress(
    [property: JsonPropertyName("streetName")] string StreetName,
    [property: JsonPropertyName("streetNumber")] string StreetNumber,
    [property: JsonPropertyName("formattedAddress")] string FormattedAddress,
    [property: JsonPropertyName("neighborhood")] string Neighborhood,
    [property: JsonPropertyName("complement")] string Complement,
    [property: JsonPropertyName("reference")] string Reference,
    [property: JsonPropertyName("postalCode")] string PostalCode,
    [property: JsonPropertyName("city")] string City,
    [property: JsonPropertyName("state")] string State,
    [property: JsonPropertyName("country")] string Country,
    [property: JsonPropertyName("coordinates")] Coordinates Coordinates
);