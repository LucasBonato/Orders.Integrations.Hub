using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.Request;

public record CancellationMetadata(
    [property: JsonPropertyName("skus")] List<string>? Skus
);