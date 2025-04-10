using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Dtos.Entity.Item;

public record OrderItemOption(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("index")] int Index,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("externalCode")] string ExternalCode,
    [property: JsonPropertyName("imageUrl")] string ImageUrl,
    [property: JsonPropertyName("unit")] string Unit,
    [property: JsonPropertyName("ean")] string Ean,
    [property: JsonPropertyName("quantity")] int Quantity,
    [property: JsonPropertyName("specialInstructions")] string SpecialInstructions,
    [property: JsonPropertyName("unitPrice")] Price UnitPrice,
    [property: JsonPropertyName("totalPrice")] Price TotalPrice
);