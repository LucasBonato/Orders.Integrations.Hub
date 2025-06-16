namespace BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderCreateUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}