using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;
using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public record SendNotificationEvent(
    OrderUpdateStatus Message,
    string? TopicArn
) : IEvent;