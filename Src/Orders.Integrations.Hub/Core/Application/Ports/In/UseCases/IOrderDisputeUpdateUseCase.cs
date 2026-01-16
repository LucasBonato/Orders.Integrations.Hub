using Orders.Integrations.Hub.Core.Application.Events;

namespace Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

public interface IOrderDisputeUpdateUseCase
{
    Task ProcessDispute(ProcessOrderDisputeEvent orderDisputeEvent);
}