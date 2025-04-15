using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Delivery;

public record Delivery(
    [property: JsonPropertyName("mode")] Mode Mode,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("deliveredBy")] DeliveredBy DeliveredBy,
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime,
    [property: JsonPropertyName("deliveryAddress")] DeliveryAddress DeliveryAddress,
    [property: JsonPropertyName("observations")] string Observations,
    [property: JsonPropertyName("pickupCode")] string PickupCode
);