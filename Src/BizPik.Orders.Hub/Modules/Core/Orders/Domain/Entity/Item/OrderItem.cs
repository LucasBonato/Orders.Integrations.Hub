using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Item;

public record OrderItem(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("index")] int Index,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("externalCode")] string ExternalCode,
    [property: JsonPropertyName("imageUrl")] string ImageUrl,
    [property: JsonPropertyName("unit")] OrderUnit Unit,
    [property: JsonPropertyName("ean")] string Ean,
    [property: JsonPropertyName("quantity")] int Quantity,
    [property: JsonPropertyName("specialInstructions")] string SpecialInstructions,
    [property: JsonPropertyName("unitPrice")] Price UnitPrice,
    [property: JsonPropertyName("totalPrice")] Price TotalPrice,
    [property: JsonPropertyName("optionsPrice")] Price OptionsPrice,
    [property: JsonPropertyName("options")] IReadOnlyList<OrderItemOption> Options
);