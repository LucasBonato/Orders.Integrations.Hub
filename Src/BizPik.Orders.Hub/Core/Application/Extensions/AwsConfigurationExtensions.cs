using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.SimpleNotificationService;

using BizPik.AWS.Credentials;

namespace BizPik.Orders.Hub.Core.Application.Extensions;

public static class AwsConfigurationExtensions
{
    private static string Region => AppEnv.AWS_REGION.GetDefault("us-east-1");
    private static bool IsLocalStack => AppEnv.LOCALSTACK.AWS.IS_LOCALSTACK.GetDefault(false);
    private static string LocalStackEndpointUrl => AppEnv.LOCALSTACK.ENDPOINT_URL.GetDefault("http://localhost:4566");
    private static string Profile => !IsLocalStack
        ? AppEnv.AWS_PROFILE.GetDefault("default")
        : "localstack";

    public static AmazonSimpleNotificationServiceClient SimpleNotificationServiceConfiguration()
    {
        bool isLocalSns = AppEnv.PUB_SUB.TOPICS.IS_LOCAL.GetDefault(false);
        if (IsLocalStack && isLocalSns) {
            AmazonSimpleNotificationServiceConfig config = new() {
                ServiceURL = LocalStackEndpointUrl,
                RegionEndpoint = RegionEndpoint.GetBySystemName(Region)
            };

            return new AmazonSimpleNotificationServiceClient(LoadProfileCredentials(), config);
        }

        return new AmazonSimpleNotificationServiceClient(LoadCredentials());
    }

    public static IAmazonS3 SimpleStorageServiceConfiguration()
    {
        if (IsLocalStack) {
            AmazonS3Config config = new() {
                ServiceURL = LocalStackEndpointUrl,
                ForcePathStyle = true,
                RegionEndpoint = RegionEndpoint.GetBySystemName(Region)
            };

            return new AmazonS3Client(LoadProfileCredentials(), config);

        }

        return new AmazonS3Client(LoadCredentials());
    }

    private static AWSCredentials LoadProfileCredentials()
    {
        if (!new CredentialProfileStoreChain().TryGetAWSCredentials(Profile, out var credentials) || credentials == null)
        {
            throw new InvalidOperationException($"Failed to load AWS credentials for profile '{Profile}'");
        }

        return credentials;
    }

    private static AWSCredentials LoadCredentials() {
        return SSOCredentials.LoadSsoCredentials(Profile);
    }
}