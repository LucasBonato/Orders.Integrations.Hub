namespace Orders.Integrations.Hub.Core.Application.Integration;

public interface IIntegrationRouter {
    TUseCase Resolve<TUseCase>(IntegrationKey key) where TUseCase : notnull;

    bool CanResolve<TUseCase>(IntegrationKey key) where TUseCase : notnull;
}