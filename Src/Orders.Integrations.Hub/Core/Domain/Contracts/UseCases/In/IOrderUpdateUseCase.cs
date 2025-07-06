namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderUpdateUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}