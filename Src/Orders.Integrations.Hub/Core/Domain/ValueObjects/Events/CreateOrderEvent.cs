using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;

using FastEndpoints;

using Order = Orders.Integrations.Hub.Core.Orders.Domain.Entity.Order;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;

public record CreateOrderEvent(
    Order Order,
    OrderSalesChannel SalesChannel = OrderSalesChannel.
) : IEvent;