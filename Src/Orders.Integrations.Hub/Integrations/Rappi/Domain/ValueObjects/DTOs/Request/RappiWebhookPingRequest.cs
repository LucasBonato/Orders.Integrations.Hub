using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookPingRequest(
    [property: JsonPropertyName("store_id")] int StoreId
);