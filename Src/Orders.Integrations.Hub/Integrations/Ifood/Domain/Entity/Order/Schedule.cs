using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Schedule(
    [property: JsonPropertyName("deliveryDateTimeStart")] DateTime DeliveryDateTimeStart,
    [property: JsonPropertyName("deliveryDateTimeEnd")] DateTime DeliveryDateTimeEnd
);