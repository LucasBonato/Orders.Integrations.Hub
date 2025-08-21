namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookUpdateUrlRequest(
    string Url,
    List<string> Stores
);