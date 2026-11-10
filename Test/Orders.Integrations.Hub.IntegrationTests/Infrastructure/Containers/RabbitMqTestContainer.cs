using DotNet.Testcontainers.Containers;
using RabbitMQ.Client;
using Testcontainers.RabbitMq;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Containers;

public sealed class RabbitMqTestContainer : IAsyncLifetime
{
    private readonly RabbitMqContainer _container = new RabbitMqBuilder("rabbitmq:alpine")
        .Build();
    private IConnection? _connection;
    private IChannel? _channel;

    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();

        ConnectionFactory connectionFactory = new() { Uri = new Uri(ConnectionString) };
        _connection = await connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.DisposeAsync();
        if (_connection is not null)
            await _connection.DisposeAsync();

        await _container.DisposeAsync();
    }

    public async Task ResetAsync(CancellationToken cancellationToken)
    {
        ExecResult result = await _container.ExecAsync(
            ["rabbitmqctl", "--quiet", "list_queues", "--no-table-headers", "name"],
            cancellationToken
        );

        if (result.ExitCode != 0)
            throw new InvalidOperationException($"Could not list RabbitMQ queues: {result.Stderr}");

        string[] queues = result.Stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        IChannel channel = _channel
            ?? throw new InvalidOperationException("RabbitMQ AMQP channel has not been initialized.");

        foreach (string queue in queues)
        {
            await channel.QueuePurgeAsync(queue, cancellationToken);
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
