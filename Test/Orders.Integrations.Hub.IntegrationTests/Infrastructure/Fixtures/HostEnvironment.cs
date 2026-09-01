namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;

public sealed record HostEnvironment(
    string RedisConnectionString,
    string RabbitMqConnectionString,
    string LocalStackEndpointUrl,
    string S3BucketName,
    string SnsTopicArn,
    string SnsQueueUrl
);
