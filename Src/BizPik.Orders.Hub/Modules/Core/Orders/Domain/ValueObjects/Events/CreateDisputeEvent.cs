using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Dispute;

using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public record CreateDisputeEvent(
    string ExternalOrderId,
    OrderDispute OrderDispute
) : IEvent;