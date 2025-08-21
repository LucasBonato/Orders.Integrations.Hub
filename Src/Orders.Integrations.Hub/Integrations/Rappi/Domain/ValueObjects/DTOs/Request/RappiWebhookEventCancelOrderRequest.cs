namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookEventCancelOrderRequest(
    string Event,
    string OrderId,
    string StoreId
);