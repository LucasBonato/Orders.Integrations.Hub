using OrderEntity = Orders.Integrations.Hub.Core.Domain.Entity.Order;

namespace Orders.Integrations.Hub.Core.Application.Commands;

public record CreateOrderCommand(
    OrderEntity Order
) : ICommand;