namespace Orders.Integrations.Hub.Modules.Integrations.Common.Application;

public interface ICreateOrderUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}