using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs;

namespace Orders.Integrations.Hub.Core.Domain.Contracts;

public interface IOrderClient
{
    Task CreateOrder(Order order);
    Task PatchOrder(OrderUpdate order);
    Task PatchOrderDispute(OrderUpdate order);
}