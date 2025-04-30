using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity;

public record DineIn(
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime
);