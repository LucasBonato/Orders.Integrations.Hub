using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderChangeStatusUseCase
{
    Task ExecuteAsync(ChangeOrderStatusRequest request);
}