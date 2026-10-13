using Orders.Integrations.Hub.TestCommon.Host;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure;

public sealed class IntegrationHostFixture : IAsyncLifetime
{
    public IntegrationTestEnvironment Environment { get; } = new();
    public WireMockServers WireMock { get; } = new();
    public AppFactory Host { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await Environment.InitializeAsync();
        try
        {
            Host = AppFactory.Create(Environment, WireMock);
        }
        catch
        {
            WireMock.Dispose();
            await Environment.DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Host.DisposeAsync();
        WireMock.Dispose();
        await Environment.DisposeAsync();
    }
}
