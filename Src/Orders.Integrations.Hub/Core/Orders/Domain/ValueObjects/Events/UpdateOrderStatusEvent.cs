using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;

public record UpdateOrderStatusEvent(
    OrderUpdate OrderUpdate,
    OrderSalesChannel SalesChannel = OrderSalesChannel.
) : IEvent;