namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;

public record Total(
    decimal AdditionalFees,
    decimal SubTotal,
    decimal DeliveryFee,
    decimal Benefits,
    decimal OrderAmount
);