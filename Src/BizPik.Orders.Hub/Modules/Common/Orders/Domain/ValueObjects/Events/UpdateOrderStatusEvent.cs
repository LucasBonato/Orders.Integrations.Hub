using BizPik.Orders.Hub.Modules.Common.Orders.Domain.ValueObjects.DTOs;

using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Domain.ValueObjects.Events;

public class UpdateOrderStatusEvent : IEvent
{
    public required OrderUpdateStatus OrderUpdateStatus { get; set; }
    public string SalesChannel { get; set; } = "None";
}