using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderUseCase
{
    Task CreateOrder(CreateOrderEvent order);
    Task UpdateOrderStatus(UpdateOrderStatusEvent order);
}