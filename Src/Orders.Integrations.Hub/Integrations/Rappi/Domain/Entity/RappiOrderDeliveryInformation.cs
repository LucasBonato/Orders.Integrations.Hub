using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderDeliveryInformation(
    [property: JsonPropertyName("city")] string City,
    [property: JsonPropertyName("complete_address")] string CompleteAddress,
    [property: JsonPropertyName("street_shortcut")] string StreetShortcut,
    [property: JsonPropertyName("federal_unit")] string FederalUnit,
    [property: JsonPropertyName("street_number")] string StreetNumber,
    [property: JsonPropertyName("neighborhood")] string Neighborhood,
    [property: JsonPropertyName("complement")] string Complement,
    [property: JsonPropertyName("postal_code")] string PostalCode,
    [property: JsonPropertyName("street_name")] string StreetName
);