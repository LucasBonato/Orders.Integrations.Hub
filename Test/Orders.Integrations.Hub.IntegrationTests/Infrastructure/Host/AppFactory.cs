using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

using Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;

/// <summary>Test host over the real application entry point.</summary>
public sealed class AppFactory : WebApplicationFactory<Program> {
    private readonly HostEnvironment? _environment;
    private readonly bool _autoAccept;
    
    public WireMockServers WireMock { get; }
    public HttpClient Http { get; }

    private AppFactory(
        HostEnvironment? environment,
        bool autoAccept
    ) {
        _environment = environment;
        _autoAccept = autoAccept;
        WireMock = new WireMockServers();
        Http = CreateClient();
    }

    public static AppFactory Create(
        HostEnvironment? environment = null,
        bool autoAccept = false
    ) => new(environment, autoAccept);
    
    public void ConfigureIntegrationContext(
        IntegrationContext integrationContext
    ) {
        TestIntegrationContextAccessor accessor = Services.GetRequiredService<TestIntegrationContextAccessor>();

        accessor.Set(integrationContext);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.UseEnvironment("Test");
        HostConfiguration.Apply(builder, _environment, WireMock);
        builder.ConfigureTestServices(services => {
            
            services.AddSingleton<TestIntegrationContextAccessor>();
            
            services.RemoveAll<IIntegrationContext>();

            services.AddScoped<IIntegrationContext>(provider => {
                TestIntegrationContextAccessor accessor = provider.GetRequiredService<TestIntegrationContextAccessor>();
                return accessor.Current
                       ?? throw new InvalidOperationException("Integration context was not configured for this test.");
            });
            
            // InternalClient is currently an in-code settings stub, not an HTTP adapter.
            // Keep its cache decorator while making settings deterministic per host.
            services.RemoveAll<IInternalClient>();
            services.AddSingleton<IInternalClient>(serviceProvider =>
                new InternalCacheClient(
                    logger: serviceProvider.GetRequiredService<ILogger<InternalClient>>(),
                    cacheService: serviceProvider.GetRequiredService<ICacheService>(),
                    inner: new TestIntegrationClient(
                        configuration: serviceProvider.GetRequiredService<IConfiguration>(),
                        autoAccept: _autoAccept
                    )
                )
            );
        });
    } 

    public new async ValueTask DisposeAsync() {
        await base.DisposeAsync();
        WireMock.Dispose();
    }
}
