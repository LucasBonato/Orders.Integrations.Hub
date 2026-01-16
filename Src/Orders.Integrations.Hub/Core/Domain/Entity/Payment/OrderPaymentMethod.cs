using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Payment;

public record OrderPaymentMethod(
    decimal Value,
    string Currency,
    MethodType Type,
    MethodMethod Method,
    MethodBrand Brand,
    string MethodInfo,
    OrderPaymentMethodTransaction? Transaction,
    decimal? ChangeFor
);