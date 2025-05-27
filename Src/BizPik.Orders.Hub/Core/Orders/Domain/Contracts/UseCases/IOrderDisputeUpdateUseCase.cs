namespace BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderDisputeUpdateUseCase<in TCounterProposal>
{
    Task ExecuteAsync(string orderId, string? counterProposalId, TCounterProposal? request);
}