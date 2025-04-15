using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;
using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public class SendNotificationEvent : IEvent
{
    public required OrderUpdateStatus Message { get; set; }
    public string? TopicArn { get; set; }
}