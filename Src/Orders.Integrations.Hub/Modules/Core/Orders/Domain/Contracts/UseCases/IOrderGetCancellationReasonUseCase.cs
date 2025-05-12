using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderGetCancellationReasonUseCase<TResponse>
{
    Task<IReadOnlyList<TResponse>> ExecuteAsync(OrderCancellationReasonRequest integrationOrder);
}