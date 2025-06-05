using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs;

using FastEndpoints;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;

public record SendNotificationEvent(
    OrderUpdate Message,
    string? TopicArn
) : IEvent;