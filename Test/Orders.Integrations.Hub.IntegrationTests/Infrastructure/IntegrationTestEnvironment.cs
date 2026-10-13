using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Containers;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure;

public sealed class IntegrationTestEnvironment : IAsyncLifetime
{
    public RabbitMqTestContainer RabbitMq { get; } = new();
    private RedisTestContainer Redis { get; } = new();
    private LocalStackTestContainer LocalStack { get; } = new();

    private string RabbitMqConnectionString => RabbitMq.ConnectionString;
    private string RedisConnectionString => Redis.ConnectionString;
    public string LocalStackEndpointUrl => LocalStack.EndpointUrl;
    public string SnsTopicArn => LocalStack.SnsTopicArn;
    public static string S3BucketName => LocalStackTestContainer.BucketName;

    public IDictionary<string, string> Configuration() => new Dictionary<string, string>  {
        ["ConnectionStrings:RabbitMq"] = RabbitMqConnectionString,
        ["ConnectionStrings:Redis"] = RedisConnectionString,
        ["Providers:Aws:ServiceUrlOverride"] = LocalStackEndpointUrl,
        ["ObjectStorage:Bucket:Name"] = S3BucketName,
        ["PubSub:Topics:AcceptOrder"] = SnsTopicArn,
    };

    public async ValueTask InitializeAsync()
    {
        Task[] starts = [
            RabbitMq.InitializeAsync().AsTask(),
            Redis.InitializeAsync().AsTask(),
            LocalStack.InitializeAsync().AsTask()
        ];

        try
        {
            await Task.WhenAll(starts);
        }
        catch
        {
            await DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        List<Exception> exceptions = [];

        await TryDisposeAsync(RabbitMq, exceptions);
        await TryDisposeAsync(Redis, exceptions);
        await TryDisposeAsync(LocalStack, exceptions);

        if (exceptions.Count > 0)
            throw new AggregateException($"DisposeAsync failed for {exceptions.Count} resource(s)", exceptions);
    }

    public async Task ResetAsync(bool isolateRabbitMq, CancellationToken cancellationToken)
    {
        if (isolateRabbitMq)
        {
            // When using vhost isolation, vhost isolates queues — no purge needed.
            return;
        }

        await Redis.ResetAsync(cancellationToken);
        await RabbitMq.ResetAsync(cancellationToken);
        await LocalStack.ResetAsync(cancellationToken);
    }

    private static async ValueTask TryDisposeAsync(IAsyncLifetime resource, List<Exception> exceptions)
    {
        try { await resource.DisposeAsync(); }
        catch (ObjectDisposedException ex) { exceptions.Add(ex); }
        catch (InvalidOperationException ex) { exceptions.Add(ex); }
    }
}
