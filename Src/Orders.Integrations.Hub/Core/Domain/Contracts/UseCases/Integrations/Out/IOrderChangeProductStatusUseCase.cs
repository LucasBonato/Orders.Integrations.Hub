namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.Out;

public interface IOrderChangeProductStatusUseCase
{
    Task Enable(object productEvent);
    Task Disable(object productEvent);
}