using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order.Item;

public record Scale(
    [property: JsonPropertyName("price")] decimal Price,
    [property: JsonPropertyName("minQuantity")] int MinQuantity
);