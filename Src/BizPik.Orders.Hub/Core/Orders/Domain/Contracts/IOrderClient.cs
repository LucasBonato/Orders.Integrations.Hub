using BizPik.Orders.Hub.Core.Orders.Domain.Entity;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Contracts;

public interface IOrderClient
{
    Task CreateOrder(Order order);
    Task UpdateOrderStatus(OrderUpdateStatus order);
}