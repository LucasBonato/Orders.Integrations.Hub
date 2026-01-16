using Orders.Integrations.Hub.Core.Application.DTOs;

namespace Orders.Integrations.Hub.Core.Application.Ports.In.Integration;

public interface IIntegrationRouter {
    TUseCase Resolve<TUseCase>(IntegrationKey key) where TUseCase : notnull;

    bool CanResolve<TUseCase>(IntegrationKey key) where TUseCase : notnull;
}