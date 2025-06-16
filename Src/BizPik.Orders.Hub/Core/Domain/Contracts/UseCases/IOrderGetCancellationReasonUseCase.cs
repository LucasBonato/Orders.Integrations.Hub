using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderGetCancellationReasonUseCase
{
    Task<List<CancellationReasonsResponse>> ExecuteAsync(string? externalOrderId);
}