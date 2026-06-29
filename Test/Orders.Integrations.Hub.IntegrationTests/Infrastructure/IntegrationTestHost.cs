using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Integrations.IFood.Application.Clients;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure;

public sealed class IntegrationTestHost : WebApplicationFactory<Program>, IAsyncDisposable
{
    private readonly Dictionary<string, string> _envVars = new() {
        ["INTERNAL__ENDPOINT__BASE_URL"] = "http://localhost:5001",
        ["ORDERS__ENDPOINT__BASE_URL"] = "http://localhost:5002",
        ["INTEGRATIONS__IFOOD__CLIENT__ID"] = "test-ifood-id",
        ["INTEGRATIONS__IFOOD__CLIENT__SECRET"] = "test-ifood-secret",
        ["INTEGRATIONS__IFOOD__ENDPOINT__BASE_URL"] = "http://localhost:5003",
        ["INTEGRATIONS__RAPPI__CLIENT__ID"] = "test-rappi-id",
        ["INTEGRATIONS__RAPPI__CLIENT__SECRET"] = "test-rappi-secret",
        ["INTEGRATIONS__RAPPI__CLIENT__AUDIENCE"] = "test-audience",
        ["INTEGRATIONS__RAPPI__ENDPOINT__BASE_URL"] = "http://localhost:5004",
        ["INTEGRATIONS__RAPPI__ENDPOINT__AUTH"] = "http://localhost:5005",
        ["INTEGRATIONS__FOOD99__CLIENT__ID"] = "test-food99-id",
        ["INTEGRATIONS__FOOD99__CLIENT__SECRET"] = "test-food99-secret",
        ["INTEGRATIONS__FOOD99__ENDPOINT__BASE_URL"] = "http://localhost:5006",
        ["CACHE__MODE"] = "Memory",
        ["CACHE__CONFIGURATIONS__CONNECTION_STRING"] = "localhost:6379",
        ["MESSAGE_BROKER__MODE"] = "Memory",
        ["MESSAGE_BROKER__CONFIGURATIONS__CONNECTION_STRING"] = "amqp://localhost:5672",
        ["PUB_SUB__TOPICS__ACCEPT_ORDER"] = "arn:aws:sns:us-east-1:123456789012:accept-order",
        ["PUB_SUB__TOPICS__IS_LOCAL"] = "true",
        ["OBJECT_STORAGE__BUCKET__NAME"] = "s3-local-bucket",
        ["AWS__IS_LOCALSTACK"] = "true",
        ["LOCALSTACK__ENDPOINT_URL"] = "http://localhost:4566",
        ["AWS_PROFILE"] = "localstack",
        ["AWS_REGION"] = "us-east-1",
        ["ASPNETCORE_ENVIRONMENT"] = "Test",
        ["OTEL_SERVICE_NAME"] = "test",
        ["OTEL_EXPORTER_OTLP_ENDPOINT"] = "http://localhost:4318",
    };

    private IInternalClient InternalClient { get; }
    private IOrderClient OrderClient { get; }
    public IIFoodClient IFoodClient { get; }
    public HttpClient Http { get; }

    private IntegrationTestHost(
        IInternalClient internalClient,
        IIFoodClient ifoodClient,
        IOrderClient orderClient,
        Dictionary<string, string>? overrides = null
    ) {
        InternalClient = internalClient;
        IFoodClient = ifoodClient;
        OrderClient = orderClient;

        if (overrides != null) {
            foreach (var kv in overrides)
                _envVars[kv.Key] = kv.Value;
        }

        foreach (var kv in _envVars)
            Environment.SetEnvironmentVariable(kv.Key, kv.Value);

        Http = CreateClient();
    }

    public static IntegrationTestHost Create(
        string localStackEndpoint = "http://localhost:4566",
        string bucketName = "s3-local-bucket",
        string snsTopicArn = "arn:aws:sns:us-east-1:123456789012:accept-order",
        string redisConnectionString = "localhost:6379"
    ) {
        IInternalClient? internalClient = Substitute.For<IInternalClient>();
        internalClient
            .GetIntegrationByExternalId(Arg.Any<string>())
            .Returns(new IntegrationResponse(
                TenantId: 1,
                IntegrationId: 1,
                Settings: new List<IntegrationSetting> {
                    new("ifood_merchant_id", "merchant-1"),
                    new("ifood_client_id", "test-client-id"),
                    new("ifood_client_secret", "test-secret"),
                    new("ifood_mode", "Distributed"),
                    new("rappi_store_id", "store-ext-1"),
                    new("rappi_client_id", "test-rappi-client-id"),
                    new("rappi_client_secret", "test-rappi-secret"),
                    new("rappi_mode", "Distributed"),
                    new("99food_app_id", "app-99-1"),
                    new("99food_client_id", "test-99-id"),
                    new("99food_client_secret", "test-99-secret"),
                    new("99food_mode", "Distributed"),
                    new("enable_auto_accept", "false"),
                }
            ));

        IIFoodClient? ifoodClient = Substitute.For<IIFoodClient>();
        ifoodClient
            .GetOrderDetails(Arg.Any<string>())
            .Returns(callInfo => TestDataFactory.CreateMinimalIFoodOrder(callInfo.Arg<string>()));

        IOrderClient? orderClient = Substitute.For<IOrderClient>();

        return new IntegrationTestHost(internalClient, ifoodClient, orderClient, new Dictionary<string, string> {
            ["LOCALSTACK__ENDPOINT_URL"] = localStackEndpoint,
            ["OBJECT_STORAGE__BUCKET__NAME"] = bucketName,
            ["PUB_SUB__TOPICS__ACCEPT_ORDER"] = snsTopicArn,
            ["CACHE__CONFIGURATIONS__CONNECTION_STRING"] = redisConnectionString,
        });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureTestServices(services => {
            services.AddSingleton(InternalClient);
            services.AddSingleton(IFoodClient);
            services.AddSingleton(OrderClient);
        });
    }

    public new async ValueTask DisposeAsync() {
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}