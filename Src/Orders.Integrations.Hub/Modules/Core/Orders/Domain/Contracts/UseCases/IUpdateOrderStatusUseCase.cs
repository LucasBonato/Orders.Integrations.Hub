namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IUpdateOrderStatusUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}