using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderStore(
    [property: JsonPropertyName("internal_id")] string InternalId,
    [property: JsonPropertyName("external_id")] string ExternalId,
    [property: JsonPropertyName("name")] string Name
);