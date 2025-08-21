namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;

public record Schedule(
    DateTime DeliveryDateTimeStart,
    DateTime DeliveryDateTimeEnd
);