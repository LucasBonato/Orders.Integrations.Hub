using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Delivery;

public record Delivery(
    Mode Mode,
    string Description,
    DeliveredBy DeliveredBy,
    DateTime DeliveryDateTime,
    DeliveryAddress DeliveryAddress,
    string Observations,
    string PickupCode
);