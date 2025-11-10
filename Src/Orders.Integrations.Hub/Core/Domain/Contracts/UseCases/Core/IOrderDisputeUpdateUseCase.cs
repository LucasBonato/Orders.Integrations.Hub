using Orders.Integrations.Hub.Core.Application.Events;

namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Core;

public interface IOrderDisputeUpdateUseCase
{
    Task ProcessDispute(ProcessOrderDisputeEvent orderDisputeEvent);
}