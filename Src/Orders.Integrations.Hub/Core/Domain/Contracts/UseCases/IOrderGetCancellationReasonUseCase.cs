using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderGetCancellationReasonUseCase
{
    Task<List<CancellationReasonsResponse>> ExecuteAsync(string? externalOrderId);
}