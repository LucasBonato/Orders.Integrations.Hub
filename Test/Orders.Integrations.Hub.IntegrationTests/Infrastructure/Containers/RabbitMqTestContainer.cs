using DotNet.Testcontainers.Containers;
using Testcontainers.RabbitMq;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Containers;

public sealed class RabbitMqTestContainer : IAsyncLifetime
{
    private readonly RabbitMqContainer _container = new RabbitMqBuilder("rabbitmq:alpine")
        .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync() => await _container.StartAsync();

    public async ValueTask DisposeAsync() => await _container.DisposeAsync();

    public async Task ResetAsync(CancellationToken cancellationToken)
    {
        ExecResult result = await _container.ExecAsync(
            ["rabbitmqctl", "--quiet", "list_queues", "--no-table-headers", "name"],
            cancellationToken
        );

        if (result.ExitCode != 0)
            throw new InvalidOperationException($"Could not list RabbitMQ queues: {result.Stderr}");

        foreach (string queue in result.Stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            ExecResult purge = await _container.ExecAsync(
                ["rabbitmqctl", "purge_queue", queue],
                cancellationToken
            );

            if (purge.ExitCode != 0)
                throw new InvalidOperationException($"Could not purge RabbitMQ queue '{queue}': {purge.Stderr}");
        }
    }

    public async Task CreateVHostAsync(string vhost, CancellationToken cancellationToken)
    {
        ExecResult result = await _container.ExecAsync(
            ["rabbitmqctl", "add_vhost", vhost],
            cancellationToken
        );

        if (result.ExitCode != 0)
            throw new InvalidOperationException($"Could not create RabbitMQ vhost '{vhost}': {result.Stderr}");
    }

    public async Task DeleteVHostAsync(string vhost, CancellationToken cancellationToken)
    {
        ExecResult result = await _container.ExecAsync(
            ["rabbitmqctl", "delete_vhost", vhost],
            cancellationToken
        );

        if (result.ExitCode != 0)
            throw new InvalidOperationException($"Could not delete RabbitMQ vhost '{vhost}': {result.Stderr}");
    }
}
