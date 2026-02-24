namespace Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

public interface IOrderUpdateUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}