namespace BizPik.Orders.Hub.Modules.Integrations.Common.Application;

public interface ICreateOrderUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}