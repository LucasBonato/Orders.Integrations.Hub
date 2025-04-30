using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;

public record BizPikIntegrationSetting(
    [property: JsonPropertyName("type")] int? Type,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("value")] string Value,
    [property: JsonPropertyName("dataType")] string DataType
);