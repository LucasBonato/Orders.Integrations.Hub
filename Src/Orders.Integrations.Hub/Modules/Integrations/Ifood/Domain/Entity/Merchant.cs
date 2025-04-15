using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity;

public record Merchant(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name
);