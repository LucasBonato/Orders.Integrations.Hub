namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Schedule(
    DateTime DeliveryDateTimeStart,
    DateTime DeliveryDateTimeEnd
);