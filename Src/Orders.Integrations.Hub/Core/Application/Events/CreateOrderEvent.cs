using FastEndpoints;

using Orders.Integrations.Hub.Core.Infrastructure.Integration;

using OrderEntity = Orders.Integrations.Hub.Core.Domain.Entity.Order;

namespace Orders.Integrations.Hub.Core.Application.Events;

public record CreateOrderEvent(
    OrderEntity Order,
    IntegrationKey SalesChannel
) : IEvent;