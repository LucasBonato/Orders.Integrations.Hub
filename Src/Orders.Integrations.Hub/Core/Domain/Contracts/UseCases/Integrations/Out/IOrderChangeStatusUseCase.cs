using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.Out;

public interface IOrderChangeStatusUseCase
{
    Task ExecuteAsync(ChangeOrderStatusRequest request);
}