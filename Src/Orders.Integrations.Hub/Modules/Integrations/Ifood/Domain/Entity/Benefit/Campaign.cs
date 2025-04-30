using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Benefit;

public record Campaign(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name
);