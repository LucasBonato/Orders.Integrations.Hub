namespace Orders.Integrations.Hub.TestCommon.Configuration;

public static class WireMockSettingsMap {
    public static Dictionary<string, string?> Build(Uri url) => new() {
        ["Clients:Internal:BaseUrl"] = url.ToString(),
        ["Clients:Orders:BaseUrl"] = url.ToString(),
        ["Integrations:IFood:Endpoint:BaseUrl"] = url.ToString(),
        ["Integrations:Rappi:Endpoint:BaseUrl"] = url.ToString(),
        ["Integrations:Rappi:Endpoint:AuthUrl"] = url.ToString(),
        ["Integrations:Food99:Endpoint:BaseUrl"] = url.ToString()
    };
}