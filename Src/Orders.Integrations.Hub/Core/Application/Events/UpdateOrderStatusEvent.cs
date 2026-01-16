using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Core.Infrastructure.Integration;

namespace Orders.Integrations.Hub.Core.Application.Events;

public record UpdateOrderStatusEvent(
    OrderUpdate OrderUpdate,
    IntegrationKey SalesChannel
) : IEvent;