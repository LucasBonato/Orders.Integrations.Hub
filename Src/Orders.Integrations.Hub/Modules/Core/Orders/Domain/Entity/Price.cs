using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity;

public record Price(
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("currency")] string Currency
);