namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

public enum IfoodFullOrderStatus
{
    KEEPALIVE,
    PLACED,
    CONFIRMED,
    SEPARATION_STARTED,
    SEPARATION_ENDED,
    READY_TO_PICKUP,
    DISPATCHED,
    CONCLUDED,
    CANCELLED
}