using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Benefit;

public record Campaign(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name
);