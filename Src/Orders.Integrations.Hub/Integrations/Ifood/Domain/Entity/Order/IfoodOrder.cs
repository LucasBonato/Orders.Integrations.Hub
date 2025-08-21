using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

/// <summary>
/// Represent an order entity aggregate (This is based on the <see href="https://developer.ifood.com.br/en-US/docs/guides/order/details/">IfoodOrder</see>)
/// </summary>
public record IfoodOrder(
    string Id,
    string DisplayId,
    IfoodOrderType OrderType,
    OrderTimingIfood OrderTiming,
    SalesChannel SalesChannel,
    Category Category,
    DateTime CreatedAt,
    DateTime PreparationStartDateTime,
    bool IsTest,
    string? ExtraInfo,
    Merchant Merchant,
    Customer.Customer Customer,
    IReadOnlyList<Item.Item> Items,
    IReadOnlyList<Benefit.Benefit>? Benefits,
    IReadOnlyList<AdditionalFee.AdditionalFee>? AdditionalFees,
    Total Total,
    Payments.Payments Payments,
    Picking? Picking,
    Delivery.Delivery? Delivery,
    Takeout? Takeout,
    DineIn? DineIn,
    Indoor? Indoor,
    Schedule? Schedule,
    AdditionalInfo? AdditionalInfo
);