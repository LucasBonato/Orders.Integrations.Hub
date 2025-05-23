using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Order.Customer;

public record Customer(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("documentNumber")] string DocumentNumber,
    [property: JsonPropertyName("ordersCountOnMerchant")] int OrdersCountOnMerchant,
    [property: JsonPropertyName("phone")] CustomerPhone? Phone,
    [property: JsonPropertyName("segmentation")] string Segmentation
);