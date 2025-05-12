using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderGetCancellationReasonUseCase<TResponse>
{
    Task<IReadOnlyList<TResponse>> ExecuteAsync(OrderCancellationReasonRequest integrationOrder);
}