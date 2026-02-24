namespace Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

public interface IOrderCreateUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder integrationOrder);
}