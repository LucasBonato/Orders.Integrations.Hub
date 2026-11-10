using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure;

public sealed class IntegrationHostFixture : IAsyncLifetime
{
    public IntegrationTestEnvironment Environment { get; } = new();
    public AppFactory Host { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await Environment.InitializeAsync();
        try
        {
            Host = AppFactory.Create(Environment);
        }
        catch
        {
            await Environment.DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Host.DisposeAsync();
        await Environment.DisposeAsync();
    }
}
