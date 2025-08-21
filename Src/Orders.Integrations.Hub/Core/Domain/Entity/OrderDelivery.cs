using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderDelivery(
    OrderDeliveredBy DeliveredBy,
    DateTime EstimatedDeliveryDateTime,
    DateTime DeliveryDateTime,
    Address.Address? DeliveryAddress,
    string? PickupCode
);