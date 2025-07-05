namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

public enum RappiWebhookEvent
{
    NEW_ORDER,
    ORDER_EVENT_CANCEL,
    ORDER_OTHER_EVENT,
    MENU_APPROVED,
    MENU_REJECTED,
    PING,
    STORE_CONNECTIVITY,
    ORDER_RT_TRACKING
}