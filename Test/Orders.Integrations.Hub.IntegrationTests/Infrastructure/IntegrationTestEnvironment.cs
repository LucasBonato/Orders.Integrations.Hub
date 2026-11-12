using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Containers;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.TestCommon.Configuration;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure;

public sealed class IntegrationTestEnvironment : IAsyncLifetime
{
    public RabbitMqTestContainer RabbitMq { get; } = new();
    private RedisTestContainer Redis { get; } = new();
    private LocalStackTestContainer LocalStack { get; } = new();
    public WireMockServers WireMock { get; } = new();

    private string RabbitMqConnectionString => RabbitMq.ConnectionString;
    private string RedisConnectionString => Redis.ConnectionString;
    public string LocalStackEndpointUrl => LocalStack.EndpointUrl;
    public string SnsTopicArn => LocalStack.SnsTopicArn;
    public static string S3BucketName => LocalStackTestContainer.BucketName;

    public IDictionary<string, string?> Configuration()
    {
        Dictionary<string, string?> configuration = WireMockSettingsMap.Build(WireMock.BaseUrl);
        configuration["ConnectionStrings:RabbitMq"] = RabbitMqConnectionString;
        configuration["ConnectionStrings:Redis"] = RedisConnectionString;
        configuration["Providers:Aws:ServiceUrlOverride"] = LocalStackEndpointUrl;
        configuration["ObjectStorage:Bucket:Name"] = S3BucketName;
        configuration["PubSub:Topics:AcceptOrder"] = SnsTopicArn;
        return configuration;
    }

    public async ValueTask InitializeAsync()
    {
        Task[] starts = [
            RabbitMq.InitializeAsync().AsTask(),
            Redis.InitializeAsync().AsTask(),
            LocalStack.InitializeAsync().AsTask(),
            WireMock.InitializeAsync().AsTask()
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
        await TryDisposeAsync(WireMock, exceptions);

        if (exceptions.Count > 0)
            throw new AggregateException($"DisposeAsync failed for {exceptions.Count} resource(s)", exceptions);
    }

    public async Task ResetAsync(bool isolateRabbitMq, CancellationToken cancellationToken)
    {
        Task rabbitMqReset = isolateRabbitMq
            ? Task.CompletedTask
            : RabbitMq.ResetAsync(cancellationToken);

        await Task.WhenAll(
            rabbitMqReset,
            Redis.ResetAsync(cancellationToken),
            LocalStack.ResetAsync(cancellationToken),
            WireMock.ResetAllAsync(cancellationToken)
        );
    }

    private static async ValueTask TryDisposeAsync(IAsyncLifetime resource, List<Exception> exceptions)
    {
        try { await resource.DisposeAsync(); }
        catch (ObjectDisposedException ex) { exceptions.Add(ex); }
        catch (InvalidOperationException ex) { exceptions.Add(ex); }
    }
}
