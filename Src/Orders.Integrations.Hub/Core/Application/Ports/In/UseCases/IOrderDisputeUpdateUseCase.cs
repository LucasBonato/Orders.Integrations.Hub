using Orders.Integrations.Hub.Core.Application.Commands;

namespace Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

public interface IOrderDisputeUpdateUseCase
{
    Task ProcessDispute(ProcessOrderDisputeCommand orderDisputeCommand);
}