using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Domain.Entity;

public record OrderDelivery(
    [property: JsonPropertyName("deliveredBy")] string DeliveredBy,
    [property: JsonPropertyName("estimatedDeliveryDateTime")] DateTime EstimatedDeliveryDateTime,
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime,
    [property: JsonPropertyName("deliveryAddress")] Address.Address? DeliveryAddress
);