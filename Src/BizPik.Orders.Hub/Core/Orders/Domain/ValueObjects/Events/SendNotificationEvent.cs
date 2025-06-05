using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;

public record SendNotificationEvent(
    OrderUpdate Message,
    string? TopicArn
) : IEvent;