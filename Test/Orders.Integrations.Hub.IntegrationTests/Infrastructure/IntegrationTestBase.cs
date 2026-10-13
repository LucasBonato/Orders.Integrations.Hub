using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Infrastructure.Serialization;
using Orders.Integrations.Hub.TestCommon.Host;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase(
    IntegrationHostFixture fixture
) : IAsyncLifetime {
    protected AppFactory Host { get; } = fixture.Host;
    protected IntegrationTestEnvironment Environment { get; } = fixture.Environment;
    private WireMockServers WireMock { get; } = fixture.WireMock;
    protected ICustomJsonSerializer Serializer { get; private set; } = null!;

    private static bool IsolateRabbitMq => false;
    private string? _rabbitMqVHost;

    public async ValueTask InitializeAsync()
    {
        Serializer = new CoreJsonSerializer();

        if (IsolateRabbitMq)  {
            _rabbitMqVHost = $"test-{Guid.NewGuid():N}";
            await Environment.RabbitMq.CreateVHostAsync(_rabbitMqVHost, TestContext.Current.CancellationToken);
        }
        else  {
            await Environment.ResetAsync(false, TestContext.Current.CancellationToken);
        }

        WireMock.ResetAll();

        // IOutputCacheStore outputCache = Host.Services.GetRequiredService<IOutputCacheStore>();
        // await outputCache.EvictByTagAsync("orders-hub-cache", TestContext.Current.CancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_rabbitMqVHost is not null)
        {
            await Environment.RabbitMq.DeleteVHostAsync(_rabbitMqVHost, TestContext.Current.CancellationToken);
        }
    }
}
