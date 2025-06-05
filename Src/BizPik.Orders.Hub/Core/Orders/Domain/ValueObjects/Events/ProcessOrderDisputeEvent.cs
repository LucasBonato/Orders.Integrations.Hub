using BizPik.Orders.Hub.Core.Orders.Domain.Entity.Dispute;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;

public record ProcessOrderDisputeEvent(
    string ExternalOrderId,
    OrderIntegration Integration,
    OrderDispute? OrderDispute,
    OrderEventType Type
) : IEvent;