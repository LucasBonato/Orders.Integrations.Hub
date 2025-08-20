using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

using Entity_Order = Orders.Integrations.Hub.Core.Domain.Entity.Order;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;

public record CreateOrderEvent(
    Entity_Order Order,
    OrderSalesChannel SalesChannel = OrderSalesChannel.INTERNAL
) : IEvent;