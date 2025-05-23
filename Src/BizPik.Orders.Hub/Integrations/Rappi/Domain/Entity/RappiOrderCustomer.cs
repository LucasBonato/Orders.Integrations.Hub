using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderCustomer(
    [property: JsonPropertyName("first_name")] string FirstName,
    [property: JsonPropertyName("last_name")] string LastName,
    [property: JsonPropertyName("phone_number")] string PhoneNumber,
    [property: JsonPropertyName("document_type")] string DocumentType,
    [property: JsonPropertyName("document_number")] string DocumentNumber
);