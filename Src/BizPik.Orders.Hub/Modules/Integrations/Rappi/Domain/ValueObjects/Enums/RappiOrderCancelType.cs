namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

public enum RappiOrderCancelType
{
    ITEM_WRONG_PRICE,
    ITEM_NOT_FOUND,
    ITEM_OUT_OF_STOCK,
    ORDER_MISSING_INFORMATION,
    ORDER_MISSING_ADDRESS_INFORMATION,
    ORDER_TOTAL_INCORRECT
}