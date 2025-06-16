using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

using FastEndpoints;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;

public record ProcessOrderDisputeEvent(
    string ExternalOrderId,
    OrderIntegration Integration,
    OrderDispute? OrderDispute,
    OrderEventType Type
) : IEvent;