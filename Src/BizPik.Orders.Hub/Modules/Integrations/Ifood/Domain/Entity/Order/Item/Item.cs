using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Order.Item;

public record Item(
    [property: JsonPropertyName("index")] int Index,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("uniqueId")] string UniqueId,
    [property: JsonPropertyName("imageUrl")] string ImageUrl,
    [property: JsonPropertyName("externalCode")] string? ExternalCode,
    [property: JsonPropertyName("ean")] string Ean,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("quantity")] int Quantity,
    [property: JsonPropertyName("unit")] [property: JsonConverter(typeof(JsonStringEnumConverter))] Unit Unit,
    [property: JsonPropertyName("unitPrice")] decimal UnitPrice,
    [property: JsonPropertyName("price")] decimal Price,
    [property: JsonPropertyName("scalePrices")] ScalePrices ScalePrices,
    [property: JsonPropertyName("optionsPrice")] decimal OptionsPrice,
    [property: JsonPropertyName("totalPrice")] decimal TotalPrice,
    [property: JsonPropertyName("observations")] string Observations,
    [property: JsonPropertyName("options")] IReadOnlyList<Option>? Options
);