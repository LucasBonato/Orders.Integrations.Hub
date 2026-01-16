using Orders.Integrations.Hub.Core.Application.DTOs.Request;

namespace Orders.Integrations.Hub.Core.Domain.Contracts.Ports.Out;

public interface IOrderChangeStatusUseCase
{
    Task ExecuteAsync(ChangeOrderStatusRequest request);
}