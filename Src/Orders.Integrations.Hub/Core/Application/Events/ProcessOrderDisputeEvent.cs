using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Integration;
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Application.Events;

public record ProcessOrderDisputeEvent(
    string ExternalOrderId,
    IntegrationKey Integration,
    OrderDispute? OrderDispute,
    OrderEventType Type
) : IEvent;