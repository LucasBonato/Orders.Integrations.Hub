using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;

namespace Orders.Integrations.Hub.Core.Application.Ports.In.Integration;

public interface IIntegrationRouter {
    TUseCase Resolve<TUseCase>(IntegrationKey key) where TUseCase : notnull;

    bool CanResolve<TUseCase>(IntegrationKey key) where TUseCase : notnull;
}