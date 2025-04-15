using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Delivery;

public record Delivery(
    [property: JsonPropertyName("mode")] Mode Mode,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("deliveredBy")] DeliveredBy DeliveredBy,
    [property: JsonPropertyName("deliveryDateTime")] DateTime DeliveryDateTime,
    [property: JsonPropertyName("deliveryAddress")] DeliveryAddress DeliveryAddress,
    [property: JsonPropertyName("observations")] string Observations,
    [property: JsonPropertyName("pickupCode")] string PickupCode
);