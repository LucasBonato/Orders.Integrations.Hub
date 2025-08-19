namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;

public interface IOrderDisputeUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder order);
}