using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderDelivery(
    [property: JsonPropertyName("deliveredBy")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderDeliveredBy DeliveredBy,
    [property: JsonPropertyName("estimatedDeliveryDateTime")] DateTime EstimatedDeliveryDateTime,
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime,
    [property: JsonPropertyName("deliveryAddress")] Address.Address? DeliveryAddress,
    [property: JsonPropertyName("pickupCode")] string? PickupCode
);