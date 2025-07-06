namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderChangeProductStatusUseCase
{
    Task Enable(object productEvent);
    Task Disable(object productEvent);
}