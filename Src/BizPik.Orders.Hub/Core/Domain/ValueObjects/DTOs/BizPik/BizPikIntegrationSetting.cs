using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.BizPik;

public record BizPikIntegrationSetting(
    [property: JsonPropertyName("type")] int? Type,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("value")] string Value,
    [property: JsonPropertyName("dataType")] string DataType
);