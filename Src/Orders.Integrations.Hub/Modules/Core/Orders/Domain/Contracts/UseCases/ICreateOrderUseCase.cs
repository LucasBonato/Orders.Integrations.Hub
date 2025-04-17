namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface ICreateOrderUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}