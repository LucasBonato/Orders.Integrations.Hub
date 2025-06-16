using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;

public record SendNotificationEvent(
    OrderUpdate Message,
    string? TopicArn
) : IEvent;