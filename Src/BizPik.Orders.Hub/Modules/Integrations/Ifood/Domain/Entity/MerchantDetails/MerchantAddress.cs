using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.MerchantDetails;

public record MerchantAddress(
    [property: JsonPropertyName("country")] string Country,
    [property: JsonPropertyName("state")] string State,
    [property: JsonPropertyName("city")] string City,
    [property: JsonPropertyName("postalCode")] string PostalCode,
    [property: JsonPropertyName("district")] string District,
    [property: JsonPropertyName("street")] string Street,
    [property: JsonPropertyName("number")] string Number,
    [property: JsonPropertyName("latitude")] decimal Latitude,
    [property: JsonPropertyName("longitude")] decimal Longitude
);