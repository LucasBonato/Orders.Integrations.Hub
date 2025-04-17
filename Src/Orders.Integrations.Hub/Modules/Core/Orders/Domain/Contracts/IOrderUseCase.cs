using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts;

public interface IOrderUseCase
{
    Task CreateOrder(CreateOrderEvent order);
    Task UpdateOrderStatus(UpdateOrderStatusEvent order);
}