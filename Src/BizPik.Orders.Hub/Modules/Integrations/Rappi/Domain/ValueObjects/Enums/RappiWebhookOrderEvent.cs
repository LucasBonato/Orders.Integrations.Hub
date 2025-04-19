namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

public enum RappiWebhookOrderEvent
{
    taken_visible_order,
    replace_storekeeper,
    ready_for_pick_up,
    domiciliary_in_store,
    hand_to_domiciliary,
    arrive,
    close_order,
    cancel_by_user,
    canceled_with_charge,
    cancel_without_charges,
    cancel_by_support,
    cancel_by_support_with_charge,
    cancel_by_application_user,
    canceled_from_cms,
    canceled_by_fraud_automation,
    canceled_store_closed,
    cancel_by_sk_with_charge
}