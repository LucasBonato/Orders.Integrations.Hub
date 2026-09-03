using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;

/// <summary>Loads committed test configuration and overrides only runtime-discovered values.</summary>
public static class HostConfiguration
{
    public static void Apply(
        IWebHostBuilder builder,
        HostEnvironment? environment,
        WireMockServers wireMock
    ) {
        Dictionary<string, string?> dynamicSettings = new() {
            ["Clients:Orders:BaseUrl"] = wireMock.Orders.Url,
            ["Integrations:IFood:Endpoint:BaseUrl"] = wireMock.IFood.Url,
            ["Integrations:Rappi:Endpoint:BaseUrl"] = wireMock.Rappi.Url,
            ["Integrations:Rappi:Endpoint:AuthUrl"] = wireMock.Rappi.Url,
            ["Integrations:Food99:Endpoint:BaseUrl"] = wireMock.Food99.Url
        };

        if (environment is not null) {
            dynamicSettings["MessageBroker:Provider"] = "RabbitMq";
            dynamicSettings["Cache:Provider"] = "Hybrid";
            dynamicSettings["ConnectionStrings:RabbitMq"] = environment.RabbitMqConnectionString;
            dynamicSettings["ConnectionStrings:Redis"] = environment.RedisConnectionString;
            dynamicSettings["Providers:Aws:ServiceUrlOverride"] = environment.LocalStackEndpointUrl;
            dynamicSettings["ObjectStorage:Bucket:Name"] = environment.S3BucketName;
            dynamicSettings["PubSub:Topics:AcceptOrder"] = environment.SnsTopicArn;
        }

        foreach ((string key, string? value) in dynamicSettings)
        {
            if (value is not null)
                builder.UseSetting(key, value);
        }

        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration
                .AddJsonFile(
                    path: "appsettings.IntegrationTest.json",
                    optional: false,
                    reloadOnChange: false
                )
                .Add(new HostSettingsConfigurationSource(builder, dynamicSettings.Keys));
        });
    }

    private sealed class HostSettingsConfigurationSource(
        IWebHostBuilder hostBuilder,
        IEnumerable<string> keys
    ) : IConfigurationSource {
        public IConfigurationProvider Build(IConfigurationBuilder configurationBuilder)
            => new HostSettingsConfigurationProvider(hostBuilder, keys);
    }

    private sealed class HostSettingsConfigurationProvider(
        IWebHostBuilder hostBuilder,
        IEnumerable<string> keys
    ) : ConfigurationProvider {
        public override void Load() {
            Data = keys
                .Select(key => (key, value: hostBuilder.GetSetting(key)))
                .Where(item => item.value is not null)
                .ToDictionary(item => item.key, item => item.value);
        }
    }
}
