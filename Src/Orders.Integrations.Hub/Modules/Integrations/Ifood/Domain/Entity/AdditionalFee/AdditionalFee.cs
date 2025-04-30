using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.AdditionalFee;

public record AdditionalFee(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("fullDescription")] string FullDescription,
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("liabilities")] IReadOnlyList<Liability> Liabilities
);