using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Application.Ports;

public class OrderUseCase(
    ILogger<OrderUseCase> logger,
    HttpClient httpClient
) : IExternalOrderUseCase {
    public Task CreateOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public Task UpdateOrderStatus(Order order)
    {
        throw new NotImplementedException();
    }
}