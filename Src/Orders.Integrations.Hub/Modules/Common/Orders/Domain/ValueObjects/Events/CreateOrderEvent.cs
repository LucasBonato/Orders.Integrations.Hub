using Order = Orders.Integrations.Hub.Modules.Common.Orders.Domain.Entity.Order;
using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Domain.ValueObjects.Events;

public class CreateOrderEvent : IEvent
{
    public required Order Order { get; set; }
    public string SalesChannel { get; set; } = "None";
}