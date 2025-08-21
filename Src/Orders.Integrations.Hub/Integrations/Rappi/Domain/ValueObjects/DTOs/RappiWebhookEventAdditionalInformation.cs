namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiWebhookEventAdditionalInformation(
    RappiWebhookEventCourierData? CourierData,
    int? EtaToStore,
    string? StorekeeperName
);