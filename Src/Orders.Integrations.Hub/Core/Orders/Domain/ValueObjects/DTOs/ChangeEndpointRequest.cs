using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.;

public record ChangeEndpointRequest(
    [property: JsonProperty("isActive")]
    [property: JsonPropertyName("isActive")] bool IsActive,
    [property: JsonProperty("settings")]
    [property: JsonPropertyName("settings")] List<Setting> Settings,
    [property: JsonProperty("userId")]
    [property: JsonPropertyName("userId")] string UserId
);