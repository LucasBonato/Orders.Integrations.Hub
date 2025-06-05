namespace Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderCreateUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}