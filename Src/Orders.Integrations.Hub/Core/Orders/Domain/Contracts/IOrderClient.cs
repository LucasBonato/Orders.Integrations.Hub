using Orders.Integrations.Hub.Core.Orders.Domain.Entity;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs;

namespace Orders.Integrations.Hub.Core.Orders.Domain.Contracts;

public interface IOrderClient
{
    Task CreateOrder(Order order);
    Task UpdateOrderStatus(OrderUpdateStatus order);
}