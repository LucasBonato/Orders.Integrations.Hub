using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderGetCancellationReasonUseCase
{
    Task<List<CancellationReasonsResponse>> ExecuteAsync(string? orderId);
}