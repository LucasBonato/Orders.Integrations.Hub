using BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;

namespace BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderDisputeUpdateUseCase
{
    Task ProcessDispute(ProcessOrderDisputeEvent orderDisputeEvent);
}