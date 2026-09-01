using WireMock.Server;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Mocks;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;

/// <summary>
/// Owns one isolated WireMock server per external HTTP boundary. A factory owns this
/// object, so every test instance receives fresh request history and stubs.
/// </summary>
public sealed class WireMockServers : IDisposable {
    public WireMockServer Orders { get; } = WireMockServer.Start();
    public WireMockServer IFood { get; } = WireMockServer.Start();
    public WireMockServer Rappi { get; } = WireMockServer.Start();
    public WireMockServer Food99 { get; } = WireMockServer.Start();

    public OrdersApiMock OrdersApi { get; }
    public IFoodApiMock IFoodApi { get; }
    public RappiApiMock RappiApi { get; }
    public Food99ApiMock Food99Api { get; }

    public WireMockServers() {
        OrdersApi = new OrdersApiMock(Orders);
        IFoodApi = new IFoodApiMock(IFood);
        RappiApi = new RappiApiMock(Rappi);
        Food99Api = new Food99ApiMock(Food99);
    }

    public void Dispose() {
        DisposeServer(Orders);
        DisposeServer(IFood);
        DisposeServer(Rappi);
        DisposeServer(Food99);
    }

    private static void DisposeServer(WireMockServer server) {
        server.Stop();
        server.Dispose();
    }
}
