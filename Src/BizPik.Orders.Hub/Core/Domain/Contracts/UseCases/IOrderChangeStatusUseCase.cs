using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;

public interface IOrderChangeStatusUseCase
{
    Task ExecuteAsync(ChangeOrderStatusRequest request);
}