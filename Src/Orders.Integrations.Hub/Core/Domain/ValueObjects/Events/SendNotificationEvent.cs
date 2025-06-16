using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs;

using FastEndpoints;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;

public record SendNotificationEvent(
    OrderUpdate Message,
    string? TopicArn
) : IEvent;