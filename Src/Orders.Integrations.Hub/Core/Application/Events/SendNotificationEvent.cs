using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.DTOs;

namespace Orders.Integrations.Hub.Core.Application.Events;

public record SendNotificationEvent(
    OrderUpdate Message,
    string? TopicArn
) : IEvent;