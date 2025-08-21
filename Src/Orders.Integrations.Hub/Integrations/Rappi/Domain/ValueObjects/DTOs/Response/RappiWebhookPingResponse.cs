namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

public record RappiWebhookPingResponse(
    string Status,
    string Description
);