using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

using Order = BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Order;
using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public record CreateOrderEvent(
    Order Order,
    OrderSalesChannel SalesChannel = OrderSalesChannel.BIZPIK
) : IEvent;