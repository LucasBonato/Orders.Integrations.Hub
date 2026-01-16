namespace Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;

public interface IOrderDisputeUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder order);
}