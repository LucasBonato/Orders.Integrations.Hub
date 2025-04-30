using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts;

public interface IOrderClient
{
    Task CreateOrder(Order order);
    Task UpdateOrderStatus(OrderUpdateStatus order);
}