using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.DTOs;

namespace Orders.Integrations.Hub.Core.Application.Events;

public record UpdateOrderStatusEvent(
    OrderUpdate OrderUpdate,
    IntegrationKey SalesChannel
) : IEvent;