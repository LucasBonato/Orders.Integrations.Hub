using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

using Order = Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity.Order;
using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public record CreateOrderEvent(
    Order Order,
    OrderSalesChannel SalesChannel = OrderSalesChannel.
) : IEvent;