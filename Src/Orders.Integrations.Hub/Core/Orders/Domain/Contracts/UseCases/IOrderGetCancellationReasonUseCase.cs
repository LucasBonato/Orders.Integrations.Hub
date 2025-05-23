using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderGetCancellationReasonUseCase
{
    Task<List<CancellationReasonsResponse>> ExecuteAsync(string? externalOrderId);
}