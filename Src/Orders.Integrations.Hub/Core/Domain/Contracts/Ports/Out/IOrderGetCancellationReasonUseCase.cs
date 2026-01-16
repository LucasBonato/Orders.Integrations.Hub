using Orders.Integrations.Hub.Core.Application.DTOs.Response;

namespace Orders.Integrations.Hub.Core.Domain.Contracts.Ports.Out;

public interface IOrderGetCancellationReasonUseCase
{
    Task<List<CancellationReasonsResponse>> ExecuteAsync(string? externalOrderId);
}