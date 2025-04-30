using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;

public record BizPikResponseWrapper<T>(
    [property: JsonPropertyName("data")] T Data
);