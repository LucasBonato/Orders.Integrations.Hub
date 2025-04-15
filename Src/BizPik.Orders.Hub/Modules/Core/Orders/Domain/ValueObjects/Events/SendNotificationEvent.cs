using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;
using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public class SendNotificationEvent : IEvent
{
    public required OrderUpdateStatus Message { get; set; }
    public string? TopicArn { get; set; }
}