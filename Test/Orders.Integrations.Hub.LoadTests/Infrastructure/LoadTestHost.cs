using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Orders.Integrations.Hub.Core;
using Orders.Integrations.Hub.Integrations;
using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.TestCommon.Configuration;

namespace Orders.Integrations.Hub.LoadTests.Infrastructure;

/// <summary>
/// Boots the real Orders.Integrations.Hub `Program` on real Kestrel — not
/// WebApplicationFactory's in-memory TestServer — so NBomber exercises the genuine
/// network/socket path. Every integration/internal base URL is pointed at the
/// single WireMockContainerHost, and OutboundHostGuard validates that before
/// Kestrel starts accepting traffic.
/// </summary>
public sealed class LoadTestHost : IAsyncDisposable
{
    public TestIntegrationContextAccessor IntegrationContextAccessor { get; }
    private WireMockContainerHost WireMock { get; }
    public string BaseUrl { get; }

    private readonly IHost _host;

    private LoadTestHost(
        WireMockContainerHost wireMock, 
        IHost host, 
        string baseUrl,
        TestIntegrationContextAccessor integrationContextAccessor
    ) {
        WireMock = wireMock;
        _host = host;
        BaseUrl = baseUrl;
        IntegrationContextAccessor = integrationContextAccessor;
    }

    public static async Task<LoadTestHost> StartAsync(
        IntegrationContext? defaultIntegrationContext = null, 
        CancellationToken ct = default
    ) {
        WireMockContainerHost wireMock = await WireMockContainerHost.StartAsync(ct);

        const string listenUrl = "http://127.0.0.1:5099";

        Dictionary<string, string?> overrides = new() {
            ["Clients:Orders:BaseUrl"] = wireMock.BaseUrl,
            ["Clients:Internal:BaseUrl"] = wireMock.BaseUrl,
            ["Integrations:IFood:Endpoint:BaseUrl"] = wireMock.BaseUrl,
            ["Integrations:IFood:Endpoint:AuthUrl"] = wireMock.BaseUrl,
            ["Integrations:Rappi:Endpoint:BaseUrl"] = wireMock.BaseUrl,
            ["Integrations:Rappi:Endpoint:AuthUrl"] = wireMock.BaseUrl,
            ["Integrations:Food99:Endpoint:BaseUrl"] = wireMock.BaseUrl,

            // Keep the load test self-contained by default — no RabbitMQ/Redis/
            // LocalStack containers required to get real HTTP-level numbers.
            // Swap to "RabbitMq"/"Hybrid" + real connection strings (same keys
            // HostConfiguration.Apply uses for IntegrationTests) if closer-to-prod
            // async/cache fidelity is needed for a specific run.
            ["MessageBroker:Provider"] = "Memory",
            ["Cache:Provider"] = "Memory",
        };
        
        WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions {
            EnvironmentName = "Test"
        });

        builder.WebHost.UseUrls(listenUrl);

        string assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".";
        string configPath = Path.Combine(assemblyDir, "appsettings.Test.json");

        builder.Configuration
            .AddJsonFile(configPath, optional: false, reloadOnChange: false)
            .AddInMemoryCollection(overrides);

        builder.Services
            .AddCore(builder.Configuration)
            .AddIntegrationsModule(builder.Configuration);
        
        TestIntegrationContextAccessor accessor = new();
        if (defaultIntegrationContext is not null)
            accessor.Set(defaultIntegrationContext);

        builder.Services.AddSingleton(accessor);

        builder.Services.RemoveAll<IIntegrationContext>();
        builder.Services.AddScoped<IIntegrationContext>(provider => {
            TestIntegrationContextAccessor a = provider.GetRequiredService<TestIntegrationContextAccessor>();
            IntegrationContext template = a.Current
                ?? throw new InvalidOperationException("Integration context was not configured for this load test.");

            // Endpoints and message handlers mutate IntegrationContext. Return a
            // request-scoped copy so concurrent partner requests cannot overwrite
            // each other's merchant/integration data.
            return new IntegrationContext
            {
                TenantId = template.TenantId,
                MerchantId = template.MerchantId,
                Integration = template.Integration
            };
        });

        WebApplication app = builder.Build();

        app.UseCore();

        OutboundHostGuard.Validate(app.Services.GetRequiredService<IConfiguration>());

        await app.StartAsync(ct);

        return new LoadTestHost(wireMock, app, listenUrl, accessor);
    }

    public async ValueTask DisposeAsync() {
        await _host.StopAsync();
        _host.Dispose();
        await WireMock.DisposeAsync();
    }
}
