using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;

public record RappiOrderItem(
    [property: JsonPropertyName("price")] decimal? Price ,
    [property: JsonPropertyName("sku")] string? Sku ,
    [property: JsonPropertyName("id")] string Id ,
    [property: JsonPropertyName("name")] string Name ,
    [property: JsonPropertyName("type")] [property: JsonConverter(typeof(JsonStringEnumConverter))] RappiOrderItemType Type,
    [property: JsonPropertyName("comments")] string? Comments ,
    [property: JsonPropertyName("unit_price_with_discount")] double? UnitPriceWithDiscount ,
    [property: JsonPropertyName("unit_price_without_discount")] double? UnitPriceWithoutDiscount ,
    [property: JsonPropertyName("percentage_discount")] double? PercentageDiscount ,
    [property: JsonPropertyName("quantity")] int Quantity ,
    [property: JsonPropertyName("subitems")] List<RappiOrderSubItem> Subitems
);