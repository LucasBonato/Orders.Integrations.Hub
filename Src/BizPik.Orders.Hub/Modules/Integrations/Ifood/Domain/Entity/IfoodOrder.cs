using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;

/// <summary>
/// Represent an order entity aggregate (This is based on the <see href="https://developer.ifood.com.br/en-US/docs/guides/order/details/">IfoodOrder</see>)
/// </summary>
public record IfoodOrder(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("displayId")] string DisplayId,
    [property: JsonPropertyName("orderType")] IfoodOrderType orderType,
    [property: JsonPropertyName("orderTiming")] OrderTimingIfood OrderTiming,
    [property: JsonPropertyName("salesChannel")] SalesChannel SalesChannel,
    [property: JsonPropertyName("category")] Category Category,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("preparationStartDateTime")] DateTime PreparationStartDateTime,
    [property: JsonPropertyName("isTest")] bool IsTest,
    [property: JsonPropertyName("extraInfo")] string ExtraInfo,
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