namespace Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

public enum Food99OrderPromoType
{
    None = 0,                                 //             // No promotion
    MinimumOrderDiscount = 1,                 // Order Level // Encourage spending by offering discounts at different price points
    SaleItemPromotion = 2,                    // Item Level  // Set a special discount price to certain items
    FreeDeliveryEvent = 3,                    // Order Level // Waives the delivery fee after spending threshold
    BuyXGetYPromotion = 4,                    // Item Level  // "Buy 1, Get 2" or similar
    OverallOrderCoupon = 10,                  // Order Level // Coupon on total order (Items + fees)
    OrderItemsCoupon = 11,                    // Order Level // Coupon on the total of the items only
    DeliveryCoupon = 12,                      // Order Level // Coupon on delivery fee
    DeliveryMemberDiscount = 20,              // Order Level // Discount for DiDi Members
    ShareDeliveryDiscount = 30,               // Order Level // Discount for shared delivery
    DidiMembershipDiscount = 34,              // Order Level // Discount for Didi membership holders
    NewUserDiscount = 100,                    // Both        // Discount for new users
    RecurrentUserDiscount = 101               // Both        // Discount for returning users
}