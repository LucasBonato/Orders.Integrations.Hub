using Orders.Integrations.Hub.Core.Application.Events;

namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Core;

public interface IOrderUseCase
{
    Task CreateOrder(CreateOrderEvent order);
    Task UpdateOrderStatus(UpdateOrderStatusEvent order);
}