using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;

public record Social(
    [property: JsonProperty("id")]
    [property: JsonPropertyName("id")] int? Id,
    [property: JsonProperty("name")]
    [property: JsonPropertyName("name")] string Name
);