using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderGetCancellationReasonUseCase
{
    Task<List<CancellationReasonsResponse>> ExecuteAsync(string? orderId);
}