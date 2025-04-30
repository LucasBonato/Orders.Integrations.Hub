using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.Providers;

public interface IOrderChangeStatusUseCaseProvider
{
    IOrderChangeStatusUseCase Get(OrderIntegration integration);
}