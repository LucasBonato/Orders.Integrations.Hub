using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Containers;
using Orders.Integrations.Hub.TestCommon.Mocks;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;

/// <summary>
/// Owns a single containerized WireMock instance and exposes API mock classes
/// for each external HTTP boundary (Orders, IFood, Rappi, Food99).
/// </summary>
public sealed class WireMockServers : IAsyncLifetime
{
    private readonly WireMockTestContainer _container = new();
    private WireMockAdminClient? _adminClient;

    public Uri BaseUrl => _container.BaseUrl;

    public OrdersApiMock OrdersApi { get; private set; } = null!;
    public IFoodApiMock IFoodApi { get; private set; } = null!;
    public RappiApiMock RappiApi { get; private set; } = null!;
    public Food99ApiMock Food99Api { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();

        Uri baseUrl = _container.BaseUrl
            ?? throw new InvalidOperationException("WireMock container URL is not available.");

        _adminClient = new WireMockAdminClient(baseUrl.AbsoluteUri);

        OrdersApi = new OrdersApiMock(_adminClient);
        IFoodApi = new IFoodApiMock(_adminClient);
        RappiApi = new RappiApiMock(_adminClient);
        Food99Api = new Food99ApiMock(_adminClient);
    }

    /// <summary>Clears mappings and request logs on the container. Call this once per test.</summary>
    public Task ResetAllAsync(CancellationToken cancellationToken = default)
        => (_adminClient ?? throw new InvalidOperationException("WireMock has not been initialized."))
            .ResetAllAsync(cancellationToken);

    public async ValueTask DisposeAsync()
    {
        _adminClient?.Dispose();
        await _container.DisposeAsync();
    }
}
