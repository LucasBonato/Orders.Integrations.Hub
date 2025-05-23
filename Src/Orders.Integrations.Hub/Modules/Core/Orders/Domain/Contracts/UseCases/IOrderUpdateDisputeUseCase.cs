namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderUpdateDisputeUseCase<in TCounterProposal>
{
    Task ExecuteAsync(string orderId, string? counterProposalId, TCounterProposal? request);
}