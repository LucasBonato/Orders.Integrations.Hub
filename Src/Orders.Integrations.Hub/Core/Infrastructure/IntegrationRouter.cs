using Orders.Integrations.Hub.Core.Application.Exceptions;
using Orders.Integrations.Hub.Core.Application.Integration;

namespace Orders.Integrations.Hub.Core.Infrastructure;

public sealed class IntegrationRouter(IServiceProvider serviceProvider) : IIntegrationRouter {
    public TUseCase Resolve<TUseCase>(IntegrationKey key) where TUseCase : notnull {
        TUseCase service = serviceProvider.GetRequiredKeyedService<TUseCase>(key);

        return service?? throw new UnknownIntegrationException(typeof(TUseCase), key);
    }

    public bool CanResolve<TUseCase>(IntegrationKey key) where TUseCase : notnull => serviceProvider.GetKeyedService<TUseCase>(key) is not null;
}