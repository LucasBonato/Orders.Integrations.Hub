using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiWebhookEventAdditionalInformation(
    [property: JsonPropertyName("courier_data")] RappiWebhookEventCourierData? CourierData,
    [property: JsonPropertyName("eta_to_store")] int? EtaToStore,
    [property: JsonPropertyName("storekeeper_name")] string? StorekeeperName
);