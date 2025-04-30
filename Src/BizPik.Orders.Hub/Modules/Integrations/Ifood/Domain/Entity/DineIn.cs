using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;

public record DineIn(
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime
);