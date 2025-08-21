namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Payments;

public record TransactionMethod(
    string AuthorizationCode,
    string AcquirerDocument
);