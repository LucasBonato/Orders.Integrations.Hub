using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;

public record UpdateOrderStatusEvent(
    OrderUpdateStatus OrderUpdateStatus,
    OrderSalesChannel SalesChannel = OrderSalesChannel.BIZPIK
) : IEvent;