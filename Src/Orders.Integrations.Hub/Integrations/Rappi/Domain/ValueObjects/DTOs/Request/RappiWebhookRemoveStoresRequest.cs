using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookRemoveStoresRequest(
    [property: JsonPropertyName("stores")] List<string> Stores
);