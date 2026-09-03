using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Aws;

public static class LocalStackAwsClients
{
    private static readonly BasicAWSCredentials Credentials = new("dummy", "dummy");

    public static AmazonS3Client S3(HostEnvironment environment)
        => new(
            Credentials, 
            new AmazonS3Config {
                ServiceURL = environment.LocalStackEndpointUrl,
                AuthenticationRegion = "us-east-1"
            }
        );

    public static AmazonSimpleNotificationServiceClient Sns(HostEnvironment environment)
        => new(
            Credentials, 
            new AmazonSimpleNotificationServiceConfig {
                ServiceURL = environment.LocalStackEndpointUrl,
                AuthenticationRegion = "us-east-1"
            }
        );

    public static AmazonSQSClient Sqs(HostEnvironment environment)
        => new(
            Credentials, 
                new AmazonSQSConfig {
                ServiceURL = environment.LocalStackEndpointUrl,
                AuthenticationRegion = "us-east-1"
            }
        );
}