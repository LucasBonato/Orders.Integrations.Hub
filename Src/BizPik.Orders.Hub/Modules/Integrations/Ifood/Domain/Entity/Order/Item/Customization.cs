using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Order.Item;

public record Customization(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("groupName")] string GroupName,
    [property: JsonPropertyName("externalCode")] string ExternalCode,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("quantity")] int Quantity,
    [property: JsonPropertyName("unitPrice")] decimal UnitPrice,
    [property: JsonPropertyName("addition")] decimal Addition,
    [property: JsonPropertyName("price")] decimal Price
);