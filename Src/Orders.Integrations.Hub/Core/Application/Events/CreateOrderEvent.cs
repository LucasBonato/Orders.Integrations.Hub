using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

using OrderEntity = Orders.Integrations.Hub.Core.Domain.Entity.Order;

namespace Orders.Integrations.Hub.Core.Application.Events;

public record CreateOrderEvent(
    OrderEntity Order,
    OrderSalesChannel SalesChannel = OrderSalesChannel.INTERNAL
) : IEvent;