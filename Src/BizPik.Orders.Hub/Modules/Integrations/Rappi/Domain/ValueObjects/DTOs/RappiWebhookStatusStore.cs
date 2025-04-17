using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiWebhookStatusStore(
    [property: JsonPropertyName("enable")] List<string>? Enable,
    [property: JsonPropertyName("disable")] List<string>? Disable
);