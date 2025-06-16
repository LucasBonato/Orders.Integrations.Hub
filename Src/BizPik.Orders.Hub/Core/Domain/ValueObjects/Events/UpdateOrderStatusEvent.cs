using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;

public record UpdateOrderStatusEvent(
    OrderUpdate OrderUpdate,
    OrderSalesChannel SalesChannel = OrderSalesChannel.BIZPIK
) : IEvent;