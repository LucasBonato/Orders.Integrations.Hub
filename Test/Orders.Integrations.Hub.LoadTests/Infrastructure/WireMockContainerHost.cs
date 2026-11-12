using System.Net.Http.Json;
using DotNet.Testcontainers.Builders;
using WireMock.Net.Testcontainers;

namespace Orders.Integrations.Hub.LoadTests.Infrastructure;

/// <summary>
/// Starts ONE isolated, containerized WireMock instance and loads every outbound
/// stub the API needs (Orders internal client, IFood, Rappi, Food99), mirroring
/// the doc's statement that "all configured HTTP integration URLs are overridden
/// to one isolated WireMock container."
///
/// This deliberately targets the container's plain HTTP admin API
/// (`POST {baseUrl}/__admin/mappings`) instead of a specific WireMock.Net client
/// library surface, because the admin API is WireMock's stable, documented,
/// language-agnostic contract — safer to depend on than a C# client method whose
/// exact shape can drift between WireMock.Net versions. The mapping JSON shape
/// below matches WireMock.Net's serialized MappingModel; if `GET {baseUrl}/__admin/mappings`
/// after startup doesn't show the expected stubs, check that shape against the
/// installed WireMock.Net.Testcontainers version's actual admin contract first.
///
/// Route paths for Orders/IFood/Rappi/Food99 command + auth endpoints are taken
/// verbatim from the existing TestCommon mock classes (OrdersApiMock, IFoodApiMock,
/// RappiApiMock, Food99ApiMock). Paths called out in the discovery doc but not
/// present in those classes yet — IFood "catalog", Rappi "webhook administration"
/// and "availability" — are marked TODO below; confirm exact routes against
/// IIFoodClient / IRappiClient before relying on them for outbound scenarios.
/// </summary>
public sealed class WireMockContainerHost : IAsyncDisposable
{
    private readonly WireMockContainer _container;
    private readonly HttpClient _adminClient;

    public string BaseUrl { get; }

    private WireMockContainerHost(WireMockContainer container, string baseUrl) {
        _container = container;
        BaseUrl = baseUrl;
        _adminClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    public static async Task<WireMockContainerHost> StartAsync(CancellationToken ct = default)
    {
        WireMockContainer container = new WireMockContainerBuilder()
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r =>
                r.ForPath("/__admin/mappings").ForStatusCode(System.Net.HttpStatusCode.OK)))
            .Build();

        await container.StartAsync(ct);

        // TODO: confirm the exact accessor for the container's externally reachable
        // URL against the installed WireMock.Net.Testcontainers version — some
        // releases expose a `GetUrl()` helper, others require composing from
        // Hostname + GetMappedPublicPort(80). Falling back to the documented,
        // version-stable Testcontainers primitives here:
        string baseUrl = container.GetPublicUrl();

        WireMockContainerHost host = new(container, baseUrl);
        await host.RegisterStubsAsync(ct);
        return host;
    }

    private async Task RegisterStubsAsync(CancellationToken ct)
    {
        foreach (object mapping in StubDefinitions.All())
        {
            HttpResponseMessage response = await _adminClient.PostAsJsonAsync("/__admin/mappings", mapping, ct);
            response.EnsureSuccessStatusCode();
        }
    }

    public async ValueTask DisposeAsync()
    {
        _adminClient.Dispose();
        await _container.StopAsync();
        await _container.DisposeAsync();
    }
}

/// <summary>
/// The stub set registered into the container at startup. Route paths mirror
/// TestCommon's WireMockApi.Respond(...) calls exactly (same integrations, same
/// paths, same wildcard style) — this is a deliberate re-expression of those
/// mocks against a remote admin API rather than a fork of their behavior.
/// </summary>
internal static class StubDefinitions
{
    public static IEnumerable<object> All()
    {
        // ---- Orders internal client (IOrderClient) ----
        yield return Mapping("POST", "/Orders", 200, "{}");
        yield return Mapping("PATCH", "/Orders", 200, "{}");
        yield return Mapping("PATCH", "/Orders/dispute", 200, "{}");

        // ---- IFood (IIFoodClient) ----
        yield return Mapping("POST", "/authentication/v1.0/oauth/token", 200,
            """{"accessToken":"ifood-token","expiresIn":3600}""");
        yield return MappingWildcard("GET", "/order/v1.0/orders/*", 200,
            """{"id":"order-1","createdAt":"2026-01-01T00:00:00Z"}""");
        yield return MappingWildcard("GET", "/order/v1.0/orders/*/cancellationReasons", 200,
            """[{"cancelCodeId":"0","description":"Customer request"}]""");
        yield return MappingWildcard("POST", "/order/v1.0/orders/*/confirm", 200, "{}");
        yield return MappingWildcard("POST", "/order/v1.0/orders/*/preparationStarted", 200, "{}");
        yield return MappingWildcard("POST", "/order/v1.0/orders/*/readyToPickup", 200, "{}");
        yield return MappingWildcard("POST", "/order/v1.0/orders/*/dispatch", 200, "{}");
        yield return MappingWildcard("POST", "/order/v1.0/orders/*/requestCancellation", 200, "{}");
        yield return MappingWildcard("POST", "/order/v1.0/disputes/*/accept", 200, "{}");
        yield return MappingWildcard("POST", "/order/v1.0/disputes/*/reject", 200, "{}");
        yield return MappingWildcard("POST", "/order/v1.0/disputes/*/alternatives/*", 200, "{}");
        // TODO: catalog routes for IIFoodClient — confirm exact paths, not covered
        // by the current TestCommon IFoodApiMock. Placeholder catch-all below keeps
        // unexpected calls from failing with connection errors in the meantime.

        // ---- Rappi (IRappiClient) ----
        yield return Mapping("POST", "/oauth/token", 200,
            """{"access_token":"rappi-token","scope":"load-test","expires_in":3600,"token_type":"Bearer"}""");
        yield return MappingWildcard("PUT", "/restaurants/orders/v1/stores/*/orders/*/take", 200, "{}");
        yield return MappingWildcard("PUT", "/orders/*/take/*", 200, "{}");
        yield return MappingWildcard("PUT", "/orders/*/reject", 200, "{}");
        yield return MappingWildcard("POST", "/orders/*/ready-for-pickup", 200, "{}");
        // TODO: "webhook administration" and "availability" routes from the
        // discovery doc's Outbound Boundaries — not present in the current
        // RappiApiMock; confirm against IRappiClient before use.

        // ---- Food99 (IFood99Client) ----
        yield return Mapping("GET", "/v1/auth/authtoken/get", 200,
            """{"errno":0,"errmsg":"success","requestId":"load-test","time":1893456000000,"data":{"app_id":1,"app_shop_id":"12345","auth_token":"food99-token","token_expiration_time":1893456000}}""");
        yield return Mapping("POST", "/v1/order/order/confirm", 200, """{"result":true}""");
        yield return Mapping("POST", "/v1/order/order/ready", 200, """{"result":true}""");
        yield return Mapping("POST", "/v1/order/order/delivered", 200, """{"result":true}""");
        yield return Mapping("POST", "/v1/order/order/cancel", 200, """{"result":true}""");

    // ---- Low-priority catch-all turns an unmapped outbound route into a clear
    // 404 and records it in WireMock's request log instead of silently succeeding. ----
        yield return CatchAll();
    }

    // Anonymous-object properties can't contain a literal hyphen, so headers use a
    // Dictionary<string,string> to serialize the real "Content-Type" key.
    private static readonly Dictionary<string, string> JsonHeaders = new() { ["Content-Type"] = "application/json" };

    private static object Mapping(string method, string path, int statusCode, string body) => new
    {
        Request = new { Methods = new[] { method }, Path = new { Matchers = new[] { new { Name = "ExactMatcher", Pattern = path } } } },
        Response = new { StatusCode = statusCode, Headers = JsonHeaders, Body = body }
    };

    private static object MappingWildcard(string method, string pathPattern, int statusCode, string body) => new
    {
        Request = new { Methods = new[] { method }, Path = new { Matchers = new[] { new { Name = "WildcardMatcher", Pattern = pathPattern } } } },
        Response = new { StatusCode = statusCode, Headers = JsonHeaders, Body = body }
    };

    private static object CatchAll() => new
    {
        Priority = 1000,
        Request = new { Path = new { Matchers = new[] { new { Name = "WildcardMatcher", Pattern = "*" } } } },
        Response = new { StatusCode = 404, Headers = JsonHeaders, Body = "{\"error\":\"No WireMock mapping matched this request\"}" }
    };
}
