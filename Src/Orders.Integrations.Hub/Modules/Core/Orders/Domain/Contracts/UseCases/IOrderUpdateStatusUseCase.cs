namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderUpdateStatusUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}