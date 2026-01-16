using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Core.Infrastructure.Integration;

namespace Orders.Integrations.Hub.Core.Application.Events;

public record ProcessOrderDisputeEvent(
    string ExternalOrderId,
    IntegrationKey Integration,
    OrderDispute? OrderDispute,
    OrderEventType Type
) : IEvent;