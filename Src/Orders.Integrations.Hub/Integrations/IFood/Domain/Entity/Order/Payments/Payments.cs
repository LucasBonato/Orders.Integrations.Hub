namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Payments;

public record Payments(
    decimal Prepaid,
    decimal Pending,
    IReadOnlyList<PaymentsMethod> Methods
);