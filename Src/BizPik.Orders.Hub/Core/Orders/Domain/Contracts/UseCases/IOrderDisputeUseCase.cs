namespace BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderDisputeUseCase<in TOrder>
{
    Task ExecuteAsync(string orderId, TOrder order);
}