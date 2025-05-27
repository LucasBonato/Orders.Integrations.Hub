namespace BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderUpdateStatusUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}