using Orders.Integrations.Hub.Core.Application.Ports.In.Integration;
using Orders.Integrations.Hub.Core.Infrastructure.Exceptions;

namespace Orders.Integrations.Hub.Core.Infrastructure.Integration;

public sealed class IntegrationRouter(IServiceProvider serviceProvider) : IIntegrationRouter {
    public TUseCase Resolve<TUseCase>(IntegrationKey key) where TUseCase : notnull {
        TUseCase? service = serviceProvider.GetKeyedService<TUseCase>(key.Value);

        return service?? throw new UnknownIntegrationException(key);
    }

    public bool CanResolve<TUseCase>(IntegrationKey key) where TUseCase : notnull => serviceProvider.GetKeyedService<TUseCase>(key.Value) is not null;
}