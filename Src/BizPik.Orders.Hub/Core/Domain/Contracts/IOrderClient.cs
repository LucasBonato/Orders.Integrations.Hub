using BizPik.Orders.Hub.Core.Domain.Entity;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs;

namespace BizPik.Orders.Hub.Core.Domain.Contracts;

public interface IOrderClient
{
    Task CreateOrder(Order order);
    Task PatchOrder(OrderUpdate order);
    Task PatchOrderDispute(OrderUpdate order);
}