namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

public enum RappiWebhookEvents
{
    NEW_ORDER,
    ORDER_EVENT_CANCEL,
    ORDER_OTHER_EVENT,
    MENU_APPROVED,
    MENU_REJECTED,
    PING,
    STORE_CONNECTIVITY
}