using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.Request;

public record CancellationMetadata(
    [property: JsonPropertyName("skus")] List<string>? Skus
);