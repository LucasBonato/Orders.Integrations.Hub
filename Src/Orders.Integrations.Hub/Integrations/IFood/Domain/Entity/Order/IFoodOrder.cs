using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;

/// <summary>
/// Represent an order entity aggregate (This is based on the <see href="https://developer.ifood.com.br/en-US/docs/guides/order/details/">IFoodOrder</see>)
/// </summary>
public record IFoodOrder(
    string Id,
    string DisplayId,
    IFoodOrderType OrderType,
    IFoodOrderTiming FoodOrderTiming,
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