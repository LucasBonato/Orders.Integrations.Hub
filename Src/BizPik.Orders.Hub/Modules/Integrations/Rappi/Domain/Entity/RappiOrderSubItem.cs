using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;

public record RappiOrderSubItem(
    [property: JsonPropertyName("sku")] string? Sku,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("type")] [property: JsonConverter(typeof(JsonStringEnumConverter))] RappiOrderItemType Type,
    [property: JsonPropertyName("price")] decimal? Price,
    [property: JsonPropertyName("percentage_discount")] double? PercentageDiscount,
    [property: JsonPropertyName("quantity")] int? Quantity
);