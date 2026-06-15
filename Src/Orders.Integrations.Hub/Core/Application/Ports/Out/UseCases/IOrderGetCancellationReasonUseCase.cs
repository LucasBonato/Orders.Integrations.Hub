using Orders.Integrations.Hub.Core.Application.DTOs.Response;

namespace Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;

public interface IOrderGetCancellationReasonUseCase
{
    Task<List<CancellationReasonsResponse>> ExecuteAsync(string? externalOrderId);
}