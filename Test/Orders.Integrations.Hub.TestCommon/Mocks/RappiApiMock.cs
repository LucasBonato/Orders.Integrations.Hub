using System.Net;

using Orders.Integrations.Hub.IntegrationTests.Payloads;

using WireMock.Server;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public sealed class RappiApiMock(WireMockServer server)
{
    private readonly IWireMockTarget? _target;

    public RappiApiMock(IWireMockTarget target) : this((WireMockServer)null!) => _target = target;

    private const string Integration = "Rappi";
    
    public void StubToken(string token = "rappi-token")
        => Respond(
            "/oauth/token",
            HttpMethod.Post,
            body: PayloadLoader.Load(
                Integration,
                "auth",
                ("accessToken", token)
            )
        );

    public void StubCommandEndpoints() {
        Respond("/restaurants/orders/*/orders/*/take", HttpMethod.Put);
        Respond("/orders/*/take/*", HttpMethod.Put);
        Respond("/orders/*/reject", HttpMethod.Put);
        Respond("/orders/*/ready-for-pickup", HttpMethod.Put);
    }

    public int RequestCount(string path) => _target?.RequestCount(path) ?? WireMockApi.RequestCount(server, path);

    private void Respond(
        string path,
        HttpMethod method,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string body = "{}",
        string contentType = "application/json"
    ) => (_target is null
        ? new Action(() => WireMockApi.Respond(server, path, method, statusCode, body, contentType))
        : new Action(() => _target.Respond(path, method, statusCode, body, contentType)))();
}
