using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Domain.Entity;

namespace Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

public interface IOrderClient
{
    Task CreateOrder(Order order);
    Task PatchOrder(OrderUpdate order);
    Task PatchOrderDispute(OrderUpdate order);
}