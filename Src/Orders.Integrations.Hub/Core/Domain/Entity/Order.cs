using Orders.Integrations.Hub.Core.Domain.Entity.Discount;
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.Entity.Item;
using Orders.Integrations.Hub.Core.Domain.Entity.Merchant;
using Orders.Integrations.Hub.Core.Domain.Entity.Payment;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record Order(
    string OrderId,
    OrderType Type,
    string DisplayId,
    string SourceAppId,
    string? SalesChannel,
    string? VirtualBrand,
    DateTime CreatedAt,
    OrderEventType LastEvent,
    OrderTiming OrderTiming,
    DateTime PreparationStartDateTime,
    OrderMerchant Merchant,
    IReadOnlyList<OrderItem> Items,
    IReadOnlyList<OrderFee> OtherFees,
    IReadOnlyList<OrderDiscount> Discounts,
    OrderTotal Total,
    OrderPayment? Payments,
    OrderTaxInvoice? TaxInvoice,
    OrderCustomer? Customer,
    OrderSchedule? Schedule,
    OrderPriority? OrderPriority,
    OrderDelivery? Delivery,
    OrderTakeout? Takeout,
    OrderIndoor? Indoor,
    bool? SendPreparing,
    bool? SendDelivered,
    bool? SendPickedUp,
    bool? SendTracking,
    string ExtraInfo,

    string ExternalId,
    string? OrderDisplayId,
    string TenantId,

    OrderDispute? Dispute
);