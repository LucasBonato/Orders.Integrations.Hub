using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Orders.Domain.Entity;

public record Phone(
    [property: JsonPropertyName("number")] string Number,
    [property: JsonPropertyName("extension")] string Extension
);