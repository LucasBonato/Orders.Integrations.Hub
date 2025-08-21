using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Payments;

public record PaymentsMethod(
    decimal Value,
    string Currency,
    IfoodMethodType Type,
    Method Method,
    Wallet? Wallet,
    Card? Card,
    Cash? Cash,
    TransactionMethod? Transaction
);