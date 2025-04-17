using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.Providers;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.Providers;

public class OrderChangeProductStatusUseCaseProvider : IOrderChangeProductStatusUseCaseProvider
{
    private readonly Dictionary<OrderIntegration, IOrderChangeProductStatusUseCase> strategies;

    public OrderChangeProductStatusUseCaseProvider(IEnumerable<IOrderChangeProductStatusUseCase> useCases)
    {
        strategies = useCases.ToDictionary(
            useCase => useCase.Integration,
            useCase => useCase
        );
    }

    public IOrderChangeProductStatusUseCase Get(OrderIntegration integration)
    {
        if (strategies.TryGetValue(integration, out var useCase))
        {
            return useCase;
        }

        throw new NotSupportedException($"Integrations not supported: {integration}");
    }
}