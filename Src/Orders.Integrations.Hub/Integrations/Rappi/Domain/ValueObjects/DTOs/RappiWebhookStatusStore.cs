namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiWebhookStatusStore(
    List<string>? Enable,
    List<string>? Disable
);