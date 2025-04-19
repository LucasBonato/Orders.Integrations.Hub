using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public record UpdateOrderStatusEvent(
    OrderUpdateStatus OrderUpdateStatus,
    OrderSalesChannel SalesChannel = OrderSalesChannel.
) : IEvent;