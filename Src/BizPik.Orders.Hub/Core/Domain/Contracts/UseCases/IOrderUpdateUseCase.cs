namespace BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderUpdateUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}