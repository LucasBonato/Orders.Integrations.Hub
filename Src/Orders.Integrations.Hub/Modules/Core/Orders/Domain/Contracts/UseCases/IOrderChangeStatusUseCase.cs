using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderChangeStatusUseCase
{
    Task ExecuteAsync(ChangeOrderStatusRequest request);
}