namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Total(
    decimal AdditionalFees,
    decimal SubTotal,
    decimal DeliveryFee,
    decimal Benefits,
    decimal OrderAmount
);