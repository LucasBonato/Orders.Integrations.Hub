namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Payments;

public record TransactionMethod(
    string AuthorizationCode,
    string AcquirerDocument
);