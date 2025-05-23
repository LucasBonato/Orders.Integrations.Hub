using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core..Domain.ValueObjects;

public record IntegrationSetting(
    [property: JsonPropertyName("type")] int? Type,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("value")] string Value,
    [property: JsonPropertyName("dataType")] string DataType
);