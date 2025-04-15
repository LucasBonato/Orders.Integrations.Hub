namespace BizPik.Orders.Hub.Modules.Integrations.Common.Application;

public interface IUpdateOrderStatusUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}