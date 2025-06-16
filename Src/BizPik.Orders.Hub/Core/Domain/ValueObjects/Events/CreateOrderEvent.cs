using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;

using FastEndpoints;

using Order = BizPik.Orders.Hub.Core.Domain.Entity.Order;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;

public record CreateOrderEvent(
    Order Order,
    OrderSalesChannel SalesChannel = OrderSalesChannel.BIZPIK
) : IEvent;