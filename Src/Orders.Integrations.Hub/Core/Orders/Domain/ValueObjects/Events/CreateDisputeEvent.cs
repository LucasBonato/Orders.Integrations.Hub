using Orders.Integrations.Hub.Core.Orders.Domain.Entity.Dispute;

using FastEndpoints;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;

public record CreateDisputeEvent(
    string ExternalOrderId,
    OrderDispute OrderDispute
) : IEvent;