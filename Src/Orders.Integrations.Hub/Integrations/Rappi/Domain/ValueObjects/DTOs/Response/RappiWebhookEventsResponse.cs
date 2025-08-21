namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

public record RappiWebhookEventsResponse(
    string Event,
    List<RappiWebhookStore> Stores
);