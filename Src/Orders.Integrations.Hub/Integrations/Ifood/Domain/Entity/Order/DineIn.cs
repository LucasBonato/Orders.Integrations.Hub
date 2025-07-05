using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record DineIn(
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime
);