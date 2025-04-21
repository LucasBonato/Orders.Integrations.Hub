using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;

public record Setting(
    [property: JsonProperty("type")]
    [property: JsonPropertyName("type")] string Type,
    [property: JsonProperty("name")]
    [property: JsonPropertyName("name")] string Name,
    [property: JsonProperty("value")]
    [property: JsonPropertyName("value")] string Value,
    [property: JsonProperty("data_type")]
    [property: JsonPropertyName("data_type")] string DataType
);