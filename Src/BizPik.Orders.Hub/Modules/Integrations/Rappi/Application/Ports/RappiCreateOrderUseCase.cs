using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Ports;

public class RappiCreateOrderUseCase : ICreateOrderUseCase<RappiOrder>
{
    public Task<RappiOrder> ExecuteAsync(RappiOrder integrationOrder)
    {
        throw new NotImplementedException();
    }
}