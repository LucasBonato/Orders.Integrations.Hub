using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderCustomer(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("documentNumber")] string DocumentNumber,
    [property: JsonPropertyName("email")] string? Email,
    [property: JsonPropertyName("phone")] Phone Phone,
    [property: JsonPropertyName("ordersCountOnMerchant")] int OrdersCountOnMerchant
);