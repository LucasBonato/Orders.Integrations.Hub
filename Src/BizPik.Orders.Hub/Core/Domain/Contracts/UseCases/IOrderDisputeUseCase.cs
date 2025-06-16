namespace BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderDisputeUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder order);
}