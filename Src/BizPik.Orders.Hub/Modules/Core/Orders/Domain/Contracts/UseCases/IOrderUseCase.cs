using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderUseCase
{
    Task CreateOrder(CreateOrderEvent order);
    Task UpdateOrderStatus(UpdateOrderStatusEvent order);
}