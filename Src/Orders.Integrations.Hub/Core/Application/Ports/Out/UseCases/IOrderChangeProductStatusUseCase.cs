namespace Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;

public interface IOrderChangeProductStatusUseCase
{
    Task Enable(object productEvent);
    Task Disable(object productEvent);
}