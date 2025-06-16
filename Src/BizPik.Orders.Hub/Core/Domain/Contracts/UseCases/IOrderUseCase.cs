using BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;

namespace BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderUseCase
{
    Task CreateOrder(CreateOrderEvent order);
    Task UpdateOrderStatus(UpdateOrderStatusEvent order);
}