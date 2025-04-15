using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

using Order = Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity.Order;
using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public class CreateOrderEvent : IEvent
{
    public required Order Order { get; set; }
    public OrderSalesChannel SalesChannel { get; set; } = OrderSalesChannel.;
}