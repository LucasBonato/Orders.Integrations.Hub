using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Integrations.Rappi.Domain.Entity;

namespace Orders.Integrations.Hub.Modules.Integrations.Rappi.Application.Ports;

public class RappiCreateOrderUseCase : ICreateOrderUseCase<RappiOrder>
{
    public Task<RappiOrder> ExecuteAsync(RappiOrder integrationOrder)
    {
        throw new NotImplementedException();
    }
}