using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;

public abstract class IntegrationTestBase(
    TestInfrastructure? infrastructure = null    
) : IAsyncLifetime {
    protected AppFactory Host { get; private set; } = null!;
    protected TestInfrastructure? Infrastructure => infrastructure;

    public async ValueTask InitializeAsync() {
        if (infrastructure is not null)
            await infrastructure.ResetAsync(TestContext.Current.CancellationToken);
        Host = AppFactory.Create(infrastructure?.Environment);
    }

    public async ValueTask DisposeAsync() {
        GC.SuppressFinalize(this);
        await Host.DisposeAsync();
    }
}
