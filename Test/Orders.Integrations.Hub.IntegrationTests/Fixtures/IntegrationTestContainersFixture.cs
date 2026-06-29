using Orders.Integrations.Hub.IntegrationTests.Infrastructure;

namespace Orders.Integrations.Hub.IntegrationTests.Fixtures;

public sealed class IntegrationTestContainersFixture : IAsyncLifetime {
    private readonly LocalStackContainerFixture _localStack = new();
    private readonly RedisContainerFixture _redis = new();

    private string LocalStackEndpoint => _localStack.EndpointUrl;
    private string BucketName => _localStack.BucketName;
    private string SnsTopicArn => _localStack.SnsTopicArn;
    private string RedisConnectionString => _redis.ConnectionString;

    private IntegrationTestHost Host { get; set; } = null!;

    public async ValueTask InitializeAsync() {
        await _localStack.InitializeAsync();
        try
        {
            await _redis.InitializeAsync();
        }
        catch
        {
            await _localStack.DisposeAsync();
            throw;
        }
        Host = IntegrationTestHost.Create(LocalStackEndpoint, BucketName, SnsTopicArn, RedisConnectionString);
    }

    public async ValueTask DisposeAsync() {
        List<Exception> exceptions = [];

        try { await Host.DisposeAsync(); }
        catch (Exception ex) { exceptions.Add(ex); }

        try { await _localStack.DisposeAsync(); }
        catch (Exception ex) { exceptions.Add(ex); }

        try { await _redis.DisposeAsync(); }
        catch (Exception ex) { exceptions.Add(ex); }

        if (exceptions.Count > 0)
            throw new AggregateException(
                $"DisposeAsync failed for {exceptions.Count} resource(s)", exceptions);
    }
}

[CollectionDefinition(Name)]
public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationTestContainersFixture> {
    private const string Name = "IntegrationTestContainers";
}
