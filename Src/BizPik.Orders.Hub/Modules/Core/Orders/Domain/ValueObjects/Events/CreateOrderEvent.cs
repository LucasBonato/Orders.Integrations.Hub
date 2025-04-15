using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

using Order = BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Order;
using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public class CreateOrderEvent : IEvent
{
    public required Order Order { get; set; }
    public OrderSalesChannel SalesChannel { get; set; } = OrderSalesChannel.BIZPIK;
}