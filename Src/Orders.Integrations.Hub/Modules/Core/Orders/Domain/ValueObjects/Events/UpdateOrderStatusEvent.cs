using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public class UpdateOrderStatusEvent : IEvent
{
    public required OrderUpdateStatus OrderUpdateStatus { get; set; }
    public OrderSalesChannel SalesChannel { get; set; } = OrderSalesChannel.;
}