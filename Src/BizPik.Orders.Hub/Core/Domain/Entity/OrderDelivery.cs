using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Core.Domain.Entity;

public record OrderDelivery(
    [property: JsonPropertyName("deliveredBy")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderDeliveredBy DeliveredBy,
    [property: JsonPropertyName("estimatedDeliveryDateTime")] DateTime EstimatedDeliveryDateTime,
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime,
    [property: JsonPropertyName("deliveryAddress")] Address.Address? DeliveryAddress,
    [property: JsonPropertyName("pickupCode")] string? PickupCode
);