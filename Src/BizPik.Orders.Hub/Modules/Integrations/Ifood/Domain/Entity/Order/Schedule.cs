using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Order;

public record Schedule(
    [property: JsonPropertyName("deliveryDateTimeStart")] DateTime DeliveryDateTimeStart,
    [property: JsonPropertyName("deliveryDateTimeEnd")] DateTime DeliveryDateTimeEnd
);