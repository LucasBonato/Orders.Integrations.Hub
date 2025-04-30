using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;

public record RappiOrderDetails(
    [property: JsonPropertyName("discounts")] List<RappiOrderDiscount>? Discounts,
    [property: JsonPropertyName("order_id")] string OrderId,
    [property: JsonPropertyName("cooking_time")] int? CookingTime,
    [property: JsonPropertyName("min_cooking_time")] int? MinCookingTime,
    [property: JsonPropertyName("max_cooking_time")] int? MaxCookingTime,
    [property: JsonPropertyName("created_at")] DateTime CreatedAt,
    [property: JsonPropertyName("delivery_method")] [property: JsonConverter(typeof(JsonStringEnumConverter))] RappiOrderDeliveryMethod DeliveryMethod,
    [property: JsonPropertyName("payment_method")] [property: JsonConverter(typeof(JsonStringEnumConverter))] RappiOrderPaymentMethod PaymentMethod,
    [property: JsonPropertyName("billing_information")] RappiOrderBillingInformation? BillingInformation,
    [property: JsonPropertyName("delivery_information")] RappiOrderDeliveryInformation? DeliveryInformation,
    [property: JsonPropertyName("totals")] RappiOrderTotals Totals,
    [property: JsonPropertyName("items")] List<RappiOrderItem> Items,
    [property: JsonPropertyName("delivery_discount")] RappiOrderDeliveryDiscount? DeliveryDiscount
);