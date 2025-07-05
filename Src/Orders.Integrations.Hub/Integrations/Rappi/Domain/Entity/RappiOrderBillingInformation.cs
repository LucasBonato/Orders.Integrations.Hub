using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderBillingInformation(
    [property: JsonPropertyName("billing_type")] string BillingType,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("address")] string? Address,
    [property: JsonPropertyName("phone")] string? Phone,
    [property: JsonPropertyName("email")] string? Email,
    [property: JsonPropertyName("document_type")] string DocumentType,
    [property: JsonPropertyName("document_number")] string DocumentNumber
);