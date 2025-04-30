using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.Providers;

public interface IOrderChangeProductStatusUseCaseProvider
{
    IOrderChangeProductStatusUseCase Get(OrderIntegration integration);
}