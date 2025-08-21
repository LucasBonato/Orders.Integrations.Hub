namespace Orders.Integrations.Hub.Core.Domain.Entity.Payment;

public record OrderPayment(
    int Prepaid,
    decimal Pending,
    IReadOnlyList<OrderPaymentMethod> Methods
);