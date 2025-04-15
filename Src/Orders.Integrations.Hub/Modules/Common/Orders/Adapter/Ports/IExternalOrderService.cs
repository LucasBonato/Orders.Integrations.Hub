using Orders.Integrations.Hub.Modules.Common.Orders.Domain.Entity;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Adapter.Ports;

public interface IExternalOrderService
{
    Task CreateOrder(Order order);
    Task UpdateOrderStatus(Order order);
}