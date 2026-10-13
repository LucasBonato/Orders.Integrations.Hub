using System.Net;

using Orders.Integrations.Hub.IntegrationTests.Payloads;

using WireMock.Server;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public sealed class Food99ApiMock(WireMockServer server) {
    private readonly IWireMockTarget? _target;

    public Food99ApiMock(IWireMockTarget target) : this((WireMockServer)null!) => _target = target;

    private const string Integration = "Food99";
    
    private readonly string _success = PayloadLoader.Load(
        Integration,
        "success-response"
    );

    public void StubToken(string token = "food99-token")
        => Respond(
            "/v1/auth/authtoken/get", 
            HttpMethod.Get,
            body: PayloadLoader.Load(
                Integration,
                "auth",
                ("accessToken", token)
            )
        );

    public void StubCommandEndpoints() {
        Respond("/v1/order/order/confirm", HttpMethod.Post, body: _success);
        Respond("/v1/order/order/ready", HttpMethod.Post, body: _success);
        Respond("/v1/order/order/delivered", HttpMethod.Post, body: _success);
        Respond("/v1/order/order/cancel", HttpMethod.Post, body: _success);
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
