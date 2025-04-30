using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.Providers;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.Providers;

public class OrderChangeStatusUseCaseProvider : IOrderChangeStatusUseCaseProvider
{
    private readonly Dictionary<OrderIntegration, IOrderChangeStatusUseCase> strategies;

    public OrderChangeStatusUseCaseProvider(IEnumerable<IOrderChangeStatusUseCase> useCases)
    {
        strategies = useCases.ToDictionary(
            useCase => useCase.Integration,
            useCase => useCase
        );
    }

    public IOrderChangeStatusUseCase Get(OrderIntegration integration)
    {
        if (strategies.TryGetValue(integration, out var useCase))
        {
            return useCase;
        }

        throw new NotSupportedException($"Integrations not supported: {integration}");
    }
}