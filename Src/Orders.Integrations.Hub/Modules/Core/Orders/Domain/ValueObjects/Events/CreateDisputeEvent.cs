using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity.Dispute;

using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

public record CreateDisputeEvent(
    string ExternalOrderId,
    OrderDispute OrderDispute
) : IEvent;