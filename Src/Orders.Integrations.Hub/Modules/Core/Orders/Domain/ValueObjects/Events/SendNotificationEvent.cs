using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;
using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public record SendNotificationEvent(
    OrderUpdateStatus Message,
    string? TopicArn
) : IEvent;