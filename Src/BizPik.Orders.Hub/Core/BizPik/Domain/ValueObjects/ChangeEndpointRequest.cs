using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace BizPik.Orders.Hub.Core.BizPik.Domain.ValueObjects;

public record ChangeEndpointRequest(
    [property: JsonProperty("isActive")]
    [property: JsonPropertyName("isActive")] bool IsActive,
    [property: JsonProperty("settings")]
    [property: JsonPropertyName("settings")] List<Setting> Settings,
    [property: JsonProperty("userId")]
    [property: JsonPropertyName("userId")] string UserId
);