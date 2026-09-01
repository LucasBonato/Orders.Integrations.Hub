using Testcontainers.RabbitMq;
using DotNet.Testcontainers.Containers;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;

public sealed class RabbitMqContainerFixture : IAsyncLifetime
{
    private readonly RabbitMqContainer _container = new RabbitMqBuilder("rabbitmq:alpine")
        .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync() => await _container.StartAsync();

    public async ValueTask DisposeAsync() => await _container.DisposeAsync();

    public async Task PurgeQueuesAsync(CancellationToken cancellationToken) {
        ExecResult result = await _container.ExecAsync(
            ["rabbitmqctl", "--quiet", "list_queues", "--no-table-headers", "name"], 
            cancellationToken
        );

        if (result.ExitCode != 0)
            throw new InvalidOperationException($"Could not list RabbitMQ queues: {result.Stderr}");

        foreach (string queue in result.Stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)) {
            ExecResult purge = await _container.ExecAsync(
                ["rabbitmqctl", "purge_queue", queue], 
                cancellationToken
            );

            if (purge.ExitCode != 0)
                throw new InvalidOperationException($"Could not purge RabbitMQ queue '{queue}': {purge.Stderr}");
        }
    }
}
