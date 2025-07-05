using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.AdditionalFee;

public record Liability(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("percentage")] decimal Percentage
);