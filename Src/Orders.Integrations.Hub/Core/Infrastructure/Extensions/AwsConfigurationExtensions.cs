using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime.Credentials;
using Amazon.S3;
using Amazon.SimpleNotificationService;

using Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class AwsConfigurationExtensions
{
    private static string Region => AppEnv.AWS_REGION.GetDefault("us-east-1");
    private static bool IsLocalStack => AppEnv.AWS.IS_LOCALSTACK.GetDefault(false);
    private static bool IsLocalSns => AppEnv.PUB_SUB.TOPICS.IS_LOCAL.GetDefault(false);
    private static string LocalStackEndpointUrl => AppEnv.LOCALSTACK.ENDPOINT_URL.GetDefault("http://localhost:4566");
    private static string? Profile => !IsLocalStack ? null : "localstack";

    public static IServiceCollection AddAwsConfiguration(this IServiceCollection services) {
        services.AddSingleton<IAmazonSimpleNotificationService>(_ => SimpleNotificationServiceConfiguration());
        services.AddSingleton<IAmazonS3>(_ => SimpleStorageServiceConfiguration());
        services.AddSingleton<IObjectStorageClient, SimpleStorageServiceClient>();
        return services;
    }

    private static AmazonSimpleNotificationServiceClient SimpleNotificationServiceConfiguration() {
        if (!IsLocalStack || !IsLocalSns)
            return new AmazonSimpleNotificationServiceClient(LoadCredentials());

        AmazonSimpleNotificationServiceConfig config = new() {
            ServiceURL = LocalStackEndpointUrl,
            RegionEndpoint = RegionEndpoint.GetBySystemName(Region)
        };

        return new AmazonSimpleNotificationServiceClient(LoadProfileCredentials(), config);

    }

    private static AmazonS3Client SimpleStorageServiceConfiguration() {
        if (!IsLocalStack)
            return new AmazonS3Client(LoadCredentials());
        
        AmazonS3Config config = new() {
            ServiceURL = LocalStackEndpointUrl,
            ForcePathStyle = true,
            RegionEndpoint = RegionEndpoint.GetBySystemName(Region)
        };

        return new AmazonS3Client(LoadProfileCredentials(), config);
    }
    
    private static BasicAWSCredentials LoadLocalStackCredentials() 
        => new(Profile, Profile);

    private static AWSCredentials LoadProfileCredentials() {
        if (!new CredentialProfileStoreChain().TryGetAWSCredentials(Profile, out var credentials) || credentials == null)
            return LoadLocalStackCredentials();

        return credentials;
    }

    private static AWSCredentials LoadCredentials() {
        return DefaultAWSCredentialsIdentityResolver.GetCredentials();
    }
}