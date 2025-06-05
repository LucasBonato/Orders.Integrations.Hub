using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Merchant(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name
);