namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;

public interface IOrderCreateUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}