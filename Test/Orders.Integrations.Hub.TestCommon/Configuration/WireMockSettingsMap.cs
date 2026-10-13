using Orders.Integrations.Hub.TestCommon.Host;
using Orders.Integrations.Hub.TestCommon.Mocks;

namespace Orders.Integrations.Hub.TestCommon.Configuration;

public static class WireMockSettingsMap {
    public static Dictionary<string, string?> Build(WireMockServers wireMock) => new() {
        ["Clients:Internal:BaseUrl"] = wireMock.Orders.Url,
        ["Clients:Orders:BaseUrl"] = wireMock.Orders.Url,
        ["Integrations:IFood:Endpoint:BaseUrl"] = wireMock.IFood.Url,
        ["Integrations:Rappi:Endpoint:BaseUrl"] = wireMock.Rappi.Url,
        ["Integrations:Rappi:Endpoint:AuthUrl"] = wireMock.Rappi.Url,
        ["Integrations:Food99:Endpoint:BaseUrl"] = wireMock.Food99.Url
    };

    public static Dictionary<string, string?> Build(IWireMockTarget target)
        => Build(target.Url);

    public static Dictionary<string, string?> Build(Uri url) => new() {
        ["Clients:Internal:BaseUrl"] = url.ToString(),
        ["Clients:Orders:BaseUrl"] = url.ToString(),
        ["Integrations:IFood:Endpoint:BaseUrl"] = url.ToString(),
        ["Integrations:Rappi:Endpoint:BaseUrl"] = url.ToString(),
        ["Integrations:Rappi:Endpoint:AuthUrl"] = url.ToString(),
        ["Integrations:Food99:Endpoint:BaseUrl"] = url.ToString()
    };
}
