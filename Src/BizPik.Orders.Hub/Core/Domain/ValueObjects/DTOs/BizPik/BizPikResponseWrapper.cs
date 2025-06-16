using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.BizPik;

public record BizPikResponseWrapper<T>(
    [property: JsonPropertyName("data")] T Data
);