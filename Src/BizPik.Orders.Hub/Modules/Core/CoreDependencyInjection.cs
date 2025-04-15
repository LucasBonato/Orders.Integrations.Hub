using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

using BizPik.AWS.Credentials;
using BizPik.Orders.Hub.Modules.Core.BizPik.Application;
using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.Contracts;

using Newtonsoft.Json;

namespace BizPik.Orders.Hub.Modules.Core;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services
                .AddServices()
                .AddClients()
            ;
    }

    public static IApplicationBuilder UseCore(this WebApplication app)
    {
        return app;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IAmazonSimpleNotificationService>(_ => SimplesNotificationServiceConfiguration());
        return services;
    }

    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpClient<IBizPikMonolithClient, BizPikMonolithClient>(client => {
            client.BaseAddress = new Uri(AppEnv.BIZPIK.MONOLITH.ENDPOINT.BASE_URL.NotNull());
        });

        return services;
    }

    private static AmazonSimpleNotificationServiceClient SimplesNotificationServiceConfiguration()
    {
        bool isLocalSns = AppEnv.LOCAL_SNS.GetDefault<bool>(false);
        string profile;

        if (isLocalSns) {
            AmazonSimpleNotificationServiceConfig config = new();
            profile = "localstack";
            new CredentialProfileStoreChain().TryGetAWSCredentials(profile, out AWSCredentials? credentials);
            config.Profile = new Profile(profile);
            config.ServiceURL = "http://localhost:4566";
            return new AmazonSimpleNotificationServiceClient(credentials: credentials, clientConfig: config);
        }

        profile = AppEnv.AWS_PROFILE.NotNull();
        return new AmazonSimpleNotificationServiceClient(SSOCredentials.LoadSsoCredentials(profile));
    }
}