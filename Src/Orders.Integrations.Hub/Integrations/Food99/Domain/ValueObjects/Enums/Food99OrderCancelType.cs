namespace Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

public enum Food99OrderCancelType
{
    ItemSoldOut = 1010,
    StoreClosed = 1020,
    ShopTooBusy = 1030,
    NoWaterOrPower = 1040,
    CustomerCancelled = 1050,
    NoRiderAvailable = 1060,
    Other = 1080
}