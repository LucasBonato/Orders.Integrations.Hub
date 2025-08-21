using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Payments;

public record PaymentsMethod(
    decimal Value,
    string Currency,
    IFoodMethodType Type,
    Method Method,
    Wallet? Wallet,
    Card? Card,
    Cash? Cash,
    TransactionMethod? Transaction
);