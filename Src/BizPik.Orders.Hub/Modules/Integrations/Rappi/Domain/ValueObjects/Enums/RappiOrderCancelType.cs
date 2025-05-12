namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

public enum RappiOrderCancelType
{
    ITEM_WRONG_PRICE = 0,
    ITEM_NOT_FOUND = 1,
    ITEM_OUT_OF_STOCK = 2,
    ORDER_MISSING_INFORMATION = 3,
    ORDER_MISSING_ADDRESS_INFORMATION = 4,
    ORDER_TOTAL_INCORRECT = 5
}