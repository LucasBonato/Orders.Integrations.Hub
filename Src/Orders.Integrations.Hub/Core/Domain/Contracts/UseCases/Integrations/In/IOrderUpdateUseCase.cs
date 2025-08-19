namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;

public interface IOrderUpdateUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}