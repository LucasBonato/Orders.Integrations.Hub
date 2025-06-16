using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.;

public record Social(
    [property: JsonProperty("id")]
    [property: JsonPropertyName("id")] int? Id,
    [property: JsonProperty("name")]
    [property: JsonPropertyName("name")] string Name
);