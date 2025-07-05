using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

/// <summary>
/// Represent an order entity aggregate (This is based on the <see href="https://developer.ifood.com.br/en-US/docs/guides/order/details/">IfoodOrder</see>)
/// </summary>
public record IfoodOrder(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("displayId")] string DisplayId,
    [property: JsonPropertyName("orderType")] [property: JsonConverter(typeof(JsonStringEnumConverter))] IfoodOrderType OrderType,
    [property: JsonPropertyName("orderTiming")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderTimingIfood OrderTiming,
    [property: JsonPropertyName("salesChannel")] [property: JsonConverter(typeof(JsonStringEnumConverter))] SalesChannel SalesChannel,
    [property: JsonPropertyName("category")] [property: JsonConverter(typeof(JsonStringEnumConverter))] Category Category,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("preparationStartDateTime")] DateTime PreparationStartDateTime,
    [property: JsonPropertyName("isTest")] bool IsTest,
    [property: JsonPropertyName("extraInfo")] string? ExtraInfo,
    [property: JsonPropertyName("merchant")] Merchant Merchant,
    [property: JsonPropertyName("customer")] Customer.Customer Customer,
    [property: JsonPropertyName("items")] IReadOnlyList<Item.Item> Items,
    [property: JsonPropertyName("benefits")] IReadOnlyList<Benefit.Benefit>? Benefits,
    [property: JsonPropertyName("additionalFees")] IReadOnlyList<AdditionalFee.AdditionalFee>? AdditionalFees,
    [property: JsonPropertyName("total")] Total Total,
    [property: JsonPropertyName("payments")] Payments.Payments Payments,
    [property: JsonPropertyName("picking")] Picking? Picking,
    [property: JsonPropertyName("delivery")] Delivery.Delivery? Delivery,
    [property: JsonPropertyName("takeout")] Takeout? Takeout,
    [property: JsonPropertyName("dineIn")] DineIn? DineIn,
    [property: JsonPropertyName("indoor")] Indoor? Indoor,
    [property: JsonPropertyName("schedule")] Schedule? Schedule,
    [property: JsonPropertyName("additionalInfo")] AdditionalInfo? AdditionalInfo
);