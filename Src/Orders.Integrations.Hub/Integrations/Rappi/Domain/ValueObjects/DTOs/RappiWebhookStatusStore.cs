using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiWebhookStatusStore(
    [property: JsonPropertyName("enable")] List<string>? Enable,
    [property: JsonPropertyName("disable")] List<string>? Disable
);