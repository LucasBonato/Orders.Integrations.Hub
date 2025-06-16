using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.;

public record ResponseWrapper<T>(
    [property: JsonPropertyName("data")] T Data
);