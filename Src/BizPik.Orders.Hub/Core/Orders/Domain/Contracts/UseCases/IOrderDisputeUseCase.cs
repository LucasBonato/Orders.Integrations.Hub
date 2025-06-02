namespace BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderDisputeUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder order);
}