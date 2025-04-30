using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Item;

public record Scale(
    [property: JsonPropertyName("price")] decimal Price,
    [property: JsonPropertyName("minQuantity")] int MinQuantity
);