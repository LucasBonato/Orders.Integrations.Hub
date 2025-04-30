using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;

public record ResponseWrapper<T>(
    [property: JsonPropertyName("data")] T Data
);