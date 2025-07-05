using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.MerchantDetails;

public record IfoodMerchant(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("corporateName")] string CorporateName,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("averageTicket")] decimal AverageTicket,
    [property: JsonPropertyName("exclusive")] bool Exclusive,
    [property: JsonPropertyName("type")] string Type, // enum
    [property: JsonPropertyName("status")] string Status, // enum
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("address")] MerchantAddress Address,
    [property: JsonPropertyName("operations")] MerchantOperations Operations
);