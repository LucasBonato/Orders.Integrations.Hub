using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.AdditionalFee;

public record Liability(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("percentage")] decimal Percentage
);