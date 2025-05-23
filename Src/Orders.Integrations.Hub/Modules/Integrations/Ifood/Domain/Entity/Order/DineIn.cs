using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Order;

public record DineIn(
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime
);