using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Adapter.Dtos.Entity;

public record Phone(
    [property: JsonPropertyName("number")] string Number,
    [property: JsonPropertyName("extension")] string Extension
);