using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookAddStoresRequest(
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("stores")] string[] Stores
);