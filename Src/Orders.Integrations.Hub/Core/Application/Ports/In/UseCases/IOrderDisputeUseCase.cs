namespace Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

public interface IOrderDisputeUseCase<TOrder>
{
    Task<TOrder> ExecuteAsync(TOrder order);
}