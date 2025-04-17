using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public class UpdateOrderStatusEvent : IEvent
{
    public required OrderUpdateStatus OrderUpdateStatus { get; set; }
    public OrderSalesChannel SalesChannel { get; set; } = OrderSalesChannel.BIZPIK;
}