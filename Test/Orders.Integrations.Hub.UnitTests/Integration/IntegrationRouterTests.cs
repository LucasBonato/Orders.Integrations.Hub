using Microsoft.Extensions.DependencyInjection;

using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.Core.Infrastructure.Exceptions;
using Orders.Integrations.Hub.Core.Infrastructure.Integration;

namespace Orders.Integrations.Hub.UnitTests.Integration;

public class IntegrationRouterTests
{
    private const string TestKey = "TEST";

    private interface ITestUseCase {}

    public sealed class TestUseCaseImpl : ITestUseCase { }

    [Fact]
    public void Resolve_ShouldReturnService_WhenKeyedServiceRegistered()
    {
        ServiceCollection services = [];
        services.AddKeyedScoped<ITestUseCase, TestUseCaseImpl>(TestKey);
        ServiceProvider provider = services.BuildServiceProvider();
        IntegrationRouter router = new(provider);

        ITestUseCase useCase = router.Resolve<ITestUseCase>(IntegrationKey.From(TestKey));

        Assert.NotNull(useCase);
        Assert.IsType<TestUseCaseImpl>(useCase);
    }

    [Fact]
    public void Resolve_ShouldThrow_WhenServiceNotRegistered()
    {
        ServiceCollection services = [];
        ServiceProvider provider = services.BuildServiceProvider();
        IntegrationRouter router = new(provider);

        Assert.Throws<UnknownIntegrationException>(() =>
            router.Resolve<ITestUseCase>(IntegrationKey.From("MISSING")));
    }

    [Fact]
    public void CanResolve_ShouldReturnTrue_WhenServiceRegistered()
    {
        ServiceCollection services = [];
        services.AddKeyedScoped<ITestUseCase, TestUseCaseImpl>(TestKey);
        ServiceProvider provider = services.BuildServiceProvider();
        IntegrationRouter router = new (provider);

        bool result = router.CanResolve<ITestUseCase>(IntegrationKey.From(TestKey));

        Assert.True(result);
    }

    [Fact]
    public void CanResolve_ShouldReturnFalse_WhenServiceNotRegistered()
    {
        ServiceCollection services = [];
        ServiceProvider provider = services.BuildServiceProvider();
        IntegrationRouter router = new(provider);

        bool result = router.CanResolve<ITestUseCase>(IntegrationKey.From("MISSING"));

        Assert.False(result);
    }

    [Fact]
    public void Resolve_ShouldUseScopedLifetime()
    {
        ServiceCollection services = [];
        services.AddKeyedScoped<ITestUseCase, TestUseCaseImpl>(TestKey);
        ServiceProvider provider = services.BuildServiceProvider();

        using IServiceScope scope1 = provider.CreateScope();
        using IServiceScope scope2 = provider.CreateScope();

        var router1 = new IntegrationRouter(scope1.ServiceProvider);
        var router2 = new IntegrationRouter(scope2.ServiceProvider);

        ITestUseCase useCase1 = router1.Resolve<ITestUseCase>(IntegrationKey.From(TestKey));
        ITestUseCase useCase2 = router2.Resolve<ITestUseCase>(IntegrationKey.From(TestKey));

        Assert.NotSame(useCase1, useCase2);
    }
}