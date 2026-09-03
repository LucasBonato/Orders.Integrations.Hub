using System.Net;

using StackExchange.Redis;
using Testcontainers.Redis;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;

public class RedisContainerFixture : IAsyncLifetime
{
    private readonly RedisContainer _container = new RedisBuilder("redis:alpine")
        .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync() => await _container.StartAsync();

    public async ValueTask DisposeAsync() => await _container.DisposeAsync();

    public async Task FlushAsync(CancellationToken cancellationToken)
    {
        ConfigurationOptions options = ConfigurationOptions.Parse(ConnectionString);
        options.AllowAdmin = true;
        await using ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync(options);
        EndPoint[] endpoints = connection.GetEndPoints();

        foreach (EndPoint endpoint in endpoints) {
            IServer server = connection.GetServer(endpoint, cancellationToken);
            await server.FlushDatabaseAsync();
        }
    }
}
