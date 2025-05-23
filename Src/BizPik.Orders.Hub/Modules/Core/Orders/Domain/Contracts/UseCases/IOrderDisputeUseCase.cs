namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderDisputeUseCase<in TOrder>
{
    Task ExecuteAsync(string orderId, TOrder order);
}