using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Item;

public record OrderItemOption(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("index")] int Index,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("externalCode")] string ExternalCode,
    [property: JsonPropertyName("imageUrl")] string ImageUrl,
    [property: JsonPropertyName("unit")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderUnit Unit,
    [property: JsonPropertyName("ean")] string Ean,
    [property: JsonPropertyName("quantity")] int Quantity,
    [property: JsonPropertyName("specialInstructions")] string SpecialInstructions,
    [property: JsonPropertyName("unitPrice")] Price UnitPrice,
    [property: JsonPropertyName("totalPrice")] Price TotalPrice
);