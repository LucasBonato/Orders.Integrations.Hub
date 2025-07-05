using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderUseCase
{
    Task CreateOrder(CreateOrderEvent order);
    Task UpdateOrderStatus(UpdateOrderStatusEvent order);
}