using BizPik.Orders.Hub.Core.Domain.Entity.Dispute;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;

public record ProcessOrderDisputeEvent(
    string ExternalOrderId,
    OrderIntegration Integration,
    OrderDispute? OrderDispute,
    OrderEventType Type
) : IEvent;