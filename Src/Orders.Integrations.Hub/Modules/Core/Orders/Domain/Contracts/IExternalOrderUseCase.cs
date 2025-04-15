using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts;

public interface IExternalOrderUseCase
{
    Task CreateOrder(Order order);
    Task UpdateOrderStatus(Order order);
}