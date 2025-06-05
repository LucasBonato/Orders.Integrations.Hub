using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order.Item;

public record Option(
    [property: JsonPropertyName("index")] int Index,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("groupName")] string GroupName,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("externalCode")] string ExternalCode,
    [property: JsonPropertyName("quantity")] int Quantity,
    [property: JsonPropertyName("unit")] [property: JsonConverter(typeof(JsonStringEnumConverter))] Unit Unit,
    [property: JsonPropertyName("ean")] string Ean,
    [property: JsonPropertyName("unitPrice")] decimal UnitPrice,
    [property: JsonPropertyName("addition")] decimal Addition,
    [property: JsonPropertyName("price")] decimal Price,
    [property: JsonPropertyName("customizations")] IReadOnlyList<Customization> Customizations
);