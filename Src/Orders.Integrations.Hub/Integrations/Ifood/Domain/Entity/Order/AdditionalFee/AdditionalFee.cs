using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.AdditionalFee;

public record AdditionalFee(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("fullDescription")] string FullDescription,
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("liabilities")] IReadOnlyList<Liability> Liabilities
);