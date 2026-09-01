using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

using Testcontainers.LocalStack;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;

public sealed class LocalStackContainerFixture : IAsyncLifetime
{
    private const string Bucket = "s3-local-bucket";

    private readonly LocalStackContainer _container = new LocalStackBuilder("localstack/localstack:4.0.3")
        .Build();

    public string EndpointUrl => $"http://{_container.Hostname}:{_container.GetMappedPublicPort(4566)}";

    public static string BucketName => Bucket;
    public string SnsTopicArn { get; private set; } = string.Empty;

    public async ValueTask InitializeAsync() {
        await _container.StartAsync();
        try {
            using AmazonS3Client s3 = CreateS3Client();
            await s3.PutBucketAsync(new PutBucketRequest { BucketName = Bucket }, TestContext.Current.CancellationToken);

            using AmazonSimpleNotificationServiceClient sns = CreateSnsClient();
            CreateTopicResponse topic = await sns.CreateTopicAsync(
                new CreateTopicRequest { Name = "accept-order-topic" },
                TestContext.Current.CancellationToken
            );
            SnsTopicArn = topic.TopicArn;
        }
        catch {
            await _container.DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync() => await _container.DisposeAsync();

    public AmazonS3Client CreateS3Client()
        => new(
            new BasicAWSCredentials("test", "test"),
            new AmazonS3Config {
                ServiceURL = EndpointUrl,
                AuthenticationRegion = "us-east-1",
                ForcePathStyle = true
            }
        );

    private AmazonSimpleNotificationServiceClient CreateSnsClient()
        => new(
            new BasicAWSCredentials("test", "test"),
            new AmazonSimpleNotificationServiceConfig {
                ServiceURL = EndpointUrl,
                AuthenticationRegion = "us-east-1"
            }
        );
}
