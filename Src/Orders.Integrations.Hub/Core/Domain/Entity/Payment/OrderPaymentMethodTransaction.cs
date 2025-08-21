namespace Orders.Integrations.Hub.Core.Domain.Entity.Payment;

public record OrderPaymentMethodTransaction(
    string AuthorizationCode,
    string AcquirerDocument
);