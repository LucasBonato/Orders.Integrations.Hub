namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookAddStoresRequest(
    string Url,
    string[] Stores
);