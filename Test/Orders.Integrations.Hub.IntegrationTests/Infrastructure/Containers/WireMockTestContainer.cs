using System.Net;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Containers;

public sealed class WireMockTestContainer : IAsyncDisposable
{
    private readonly IContainer _container = new ContainerBuilder("wiremock/wiremock:latest")
        .WithPortBinding(8080, true)
        .WithAutoRemove(true)
        .WithCleanUp(true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(request =>
            request
                .ForPort(8080)
                .ForPath("/__admin/mappings")
                .ForStatusCode(HttpStatusCode.OK))
        )
        .Build();

    public Uri BaseUrl => new($"http://{_container.Hostname}:{_container.GetMappedPublicPort(8080)}");

    public Task StartAsync() => _container.StartAsync();

    public async ValueTask DisposeAsync() => await _container.DisposeAsync();

}
