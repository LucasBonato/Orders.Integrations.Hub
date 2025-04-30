using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookEventCancelOrderRequest(
    [property: JsonPropertyName("event")] string Event,
    [property: JsonPropertyName("order_id")] string OrderId,
    [property: JsonPropertyName("store_id")] string StoreId
);