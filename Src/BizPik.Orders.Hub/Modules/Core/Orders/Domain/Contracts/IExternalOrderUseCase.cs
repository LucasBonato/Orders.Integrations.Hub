using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;

public interface IExternalOrderUseCase
{
    Task CreateOrder(Order order);
    Task UpdateOrderStatus(Order order);
}