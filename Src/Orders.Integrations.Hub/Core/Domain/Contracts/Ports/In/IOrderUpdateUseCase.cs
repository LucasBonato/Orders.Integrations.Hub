namespace Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;

public interface IOrderUpdateUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}