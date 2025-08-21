using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderDetails(
    List<RappiOrderDiscount>? Discounts,
    string OrderId,
    int? CookingTime,
    int? MinCookingTime,
    int? MaxCookingTime,
    DateTime CreatedAt,
    RappiOrderDeliveryMethod DeliveryMethod,
    RappiOrderPaymentMethod PaymentMethod,
    RappiOrderBillingInformation? BillingInformation,
    RappiOrderDeliveryInformation? DeliveryInformation,
    RappiOrderTotals Totals,
    List<RappiOrderItem> Items,
    RappiOrderDeliveryDiscount? DeliveryDiscount
);