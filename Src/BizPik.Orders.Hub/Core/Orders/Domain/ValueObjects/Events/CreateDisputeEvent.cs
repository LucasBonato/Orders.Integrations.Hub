using BizPik.Orders.Hub.Core.Orders.Domain.Entity.Dispute;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;

public record CreateDisputeEvent(
    string ExternalOrderId,
    OrderDispute OrderDispute
) : IEvent;