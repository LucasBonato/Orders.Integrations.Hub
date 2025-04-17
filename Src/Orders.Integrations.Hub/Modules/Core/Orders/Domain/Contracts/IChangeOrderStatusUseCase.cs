using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts;

public interface IChangeOrderStatusUseCase {
    Task Execute(ChangeOrderStatusRequest request);
}