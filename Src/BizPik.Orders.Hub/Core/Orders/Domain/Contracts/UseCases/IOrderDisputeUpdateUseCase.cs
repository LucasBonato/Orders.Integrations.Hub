using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderDisputeUpdateUseCase
{
    Task ProcessDispute(ProcessOrderDisputeEvent orderDisputeEvent);
}