using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Application.Events;

public record UpdateOrderStatusEvent(
    OrderUpdate OrderUpdate,
    OrderSalesChannel SalesChannel = OrderSalesChannel.INTERNAL
) : IEvent;