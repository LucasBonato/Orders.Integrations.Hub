using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookUpdateUrlRequest(
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("stores")] List<string> Stores
);