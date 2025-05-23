using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Order.Item;

public record ScalePrices(
    [property: JsonPropertyName("defaultPrice")] decimal DefaultPrice,
    [property: JsonPropertyName("scales")] IReadOnlyList<Scale> Scales
);