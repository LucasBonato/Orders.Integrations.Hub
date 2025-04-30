using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;

public interface IOrderClient
{
    Task CreateOrder(Order order);
    Task UpdateOrderStatus(OrderUpdateStatus order);
}