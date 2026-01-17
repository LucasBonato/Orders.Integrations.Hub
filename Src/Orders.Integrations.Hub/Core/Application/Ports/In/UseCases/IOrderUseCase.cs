using Orders.Integrations.Hub.Core.Application.Commands;

namespace Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

public interface IOrderUseCase
{
    Task CreateOrder(CreateOrderCommand order);
    Task UpdateOrderStatus(UpdateOrderStatusCommand order);
}