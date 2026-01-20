using Orders.Integrations.Hub.Core.Application.DTOs.Request;

namespace Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;

public interface IOrderChangeStatusUseCase
{
    Task ExecuteAsync(ChangeOrderStatusRequest request);
}