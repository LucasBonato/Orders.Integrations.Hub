using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Credentials;
using Amazon.S3;
using Amazon.SimpleNotificationService;

using Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Infrastructure.Options;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class AwsConfigurationExtensions {
    public static IServiceCollection AddAwsConfiguration(this IServiceCollection services, IConfiguration configuration) {
        AwsProviderOptions aws = configuration
                                    .GetSection(AwsProviderOptions.SectionName)
                                    .Get<AwsProviderOptions>()
                                        ?? AwsProviderOptions.Create();
        
        services
            .AddOptions<PubSubOptions>()
            .Bind(configuration.GetSection(PubSubOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<ObjectStorageOptions>()
            .Bind(configuration.GetSection(ObjectStorageOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddSingleton<IAmazonSimpleNotificationService>(_ => CreateSnsClient(aws));
        services.AddSingleton<IAmazonS3>(_ => CreateS3Client(aws));
        services.AddSingleton<IObjectStorageClient, SimpleStorageServiceClient>();
        return services;
    }

    private static AmazonSimpleNotificationServiceClient CreateSnsClient(AwsProviderOptions options) {
        if (!options.HasEndpointOverride)
            return new AmazonSimpleNotificationServiceClient(ResolveCredentials(options));

        AmazonSimpleNotificationServiceConfig config = new() {
            ServiceURL = options.ServiceUrlOverride,
            RegionEndpoint = RegionEndpoint.GetBySystemName(options.Region)
        };

        return new AmazonSimpleNotificationServiceClient(ResolveCredentials(options), config);
    }

    private static AmazonS3Client CreateS3Client(AwsProviderOptions options) {
        if (!options.HasEndpointOverride)
            return new AmazonS3Client(ResolveCredentials(options));

        AmazonS3Config config = new() {
            ServiceURL = options.ServiceUrlOverride,
            ForcePathStyle = options.ForcePathStyle,
            RegionEndpoint = RegionEndpoint.GetBySystemName(options.Region)
        };

        return new AmazonS3Client(ResolveCredentials(options), config);
    }
    
    private static AWSCredentials ResolveCredentials(AwsProviderOptions options) => options.HasEndpointOverride 
        ? new BasicAWSCredentials(accessKey: options.Profile, secretKey: options.Profile)
        : DefaultAWSCredentialsIdentityResolver.GetCredentials();
}