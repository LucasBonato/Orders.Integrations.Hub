namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

public record RappiWebhookRemoveStoresResponse(
    List<string> Stores,
    string Message
);