using DotNet.Testcontainers.Configurations;

using Amazon.S3;
using Amazon.S3.Model;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;

public sealed class TestInfrastructure : IAsyncLifetime {
    static TestInfrastructure() {
        TestcontainersSettings.ResourceReaperEnabled = false;
    }

    private readonly LocalStackContainerFixture _localStack = new();
    private readonly RedisContainerFixture _redis = new();
    private readonly RabbitMqContainerFixture _rabbitMq = new();

    public HostEnvironment Environment { get; private set; } = null!;

    public async ValueTask InitializeAsync() {
        Task[] starts = [
            _localStack.InitializeAsync().AsTask(),
            _redis.InitializeAsync().AsTask(),
            _rabbitMq.InitializeAsync().AsTask()
        ];

        try {
            await Task.WhenAll(starts);
        }
        catch {
            await DisposeAsync();
            throw;
        }

        Environment = new HostEnvironment(
            RedisConnectionString: _redis.ConnectionString,
            RabbitMqConnectionString: _rabbitMq.ConnectionString,
            LocalStackEndpointUrl: _localStack.EndpointUrl,
            S3BucketName: LocalStackContainerFixture.BucketName,
            SnsTopicArn: _localStack.SnsTopicArn,
            SnsQueueUrl: string.Empty
        );
    }

    /// <summary>
    /// Resets shared infrastructure before a real-infrastructure test instance starts.
    /// Tests only describe business data; cleanup remains centralized here.
    /// </summary>
    public async Task ResetAsync(CancellationToken cancellationToken) {
        await _redis.FlushAsync(cancellationToken);
        await _rabbitMq.PurgeQueuesAsync(cancellationToken);

        using AmazonS3Client s3 = _localStack.CreateS3Client();
        ListObjectsV2Response objects = await s3.ListObjectsV2Async(
            new ListObjectsV2Request {
                BucketName = LocalStackContainerFixture.BucketName
            }, 
            cancellationToken
        );

        if (objects.S3Objects is not { Count: > 0 })
            return;

        await s3.DeleteObjectsAsync(
            new DeleteObjectsRequest {
                BucketName = LocalStackContainerFixture.BucketName,
                Objects = [..objects.S3Objects.Select(item => new KeyVersion { Key = item.Key })]
            }, 
            cancellationToken
        );
    }

    public async ValueTask DisposeAsync() {
        List<Exception> exceptions = [];

        await TryDisposeAsync(_localStack, exceptions);
        await TryDisposeAsync(_redis, exceptions);
        await TryDisposeAsync(_rabbitMq, exceptions);

        if (exceptions.Count > 0)
            throw new AggregateException($"DisposeAsync failed for {exceptions.Count} resource(s)", exceptions);
    }

    private static async ValueTask TryDisposeAsync(IAsyncLifetime resource, List<Exception> exceptions) {
        try { await resource.DisposeAsync(); }
        catch (Exception ex) { exceptions.Add(ex); }
    }
}

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class IntegrationTestCollection : ICollectionFixture<TestInfrastructure> {
    public const string Name = "IntegrationTestInfrastructure";
}
