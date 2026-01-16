namespace Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;

public interface IOrderCreateUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}