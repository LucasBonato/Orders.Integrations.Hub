using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.DTOs;

using OrderEntity = Orders.Integrations.Hub.Core.Domain.Entity.Order;

namespace Orders.Integrations.Hub.Core.Application.Events;

public record CreateOrderEvent(
    OrderEntity Order,
    IntegrationKey SalesChannel
) : IEvent;