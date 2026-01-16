using Orders.Integrations.Hub.Core.Infrastructure.Integration;

namespace Orders.Integrations.Hub.Core.Application.Ports.In.Integration;

public interface IIntegrationRouter {
    TUseCase Resolve<TUseCase>(IntegrationKey key) where TUseCase : notnull;

    bool CanResolve<TUseCase>(IntegrationKey key) where TUseCase : notnull;
}