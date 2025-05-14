using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;

public interface IOrderChangeStatusUseCase
{
    Task ExecuteAsync(ChangeOrderStatusRequest request);
}