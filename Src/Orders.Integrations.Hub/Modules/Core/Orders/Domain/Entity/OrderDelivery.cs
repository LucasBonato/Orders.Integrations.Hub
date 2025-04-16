using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity;

public record OrderDelivery(
    [property: JsonPropertyName("deliveredBy")] OrderDeliveredBy DeliveredBy,
    [property: JsonPropertyName("estimatedDeliveryDateTime")] DateTime EstimatedDeliveryDateTime,
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime,
    [property: JsonPropertyName("deliveryAddress")] Address.Address? DeliveryAddress
);