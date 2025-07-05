using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderDisputeUpdateUseCase
{
    Task ProcessDispute(ProcessOrderDisputeEvent orderDisputeEvent);
}