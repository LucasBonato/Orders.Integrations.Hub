using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order;

public record DineIn(
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime
);