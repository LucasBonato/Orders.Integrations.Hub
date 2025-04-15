using BizPik.Orders.Hub.Modules.Common.Orders.Domain.Entity;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Adapter.Ports;

public interface IExternalOrderService
{
    Task CreateOrder(Order order);
    Task UpdateOrderStatus(Order order);
}