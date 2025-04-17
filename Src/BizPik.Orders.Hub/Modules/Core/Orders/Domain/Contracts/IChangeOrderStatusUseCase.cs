using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;

public interface IChangeOrderStatusUseCase {
    Task Execute(ChangeOrderStatusRequest request);
}