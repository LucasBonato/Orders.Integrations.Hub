namespace Orders.Integrations.Hub.Core.Domain.Contracts.Ports.Out;

public interface IOrderChangeProductStatusUseCase
{
    Task Enable(object productEvent);
    Task Disable(object productEvent);
}