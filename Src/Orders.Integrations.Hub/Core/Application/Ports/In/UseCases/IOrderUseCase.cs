using Orders.Integrations.Hub.Core.Application.Events;

namespace Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

public interface IOrderUseCase
{
    Task CreateOrder(CreateOrderEvent order);
    Task UpdateOrderStatus(UpdateOrderStatusEvent order);
}