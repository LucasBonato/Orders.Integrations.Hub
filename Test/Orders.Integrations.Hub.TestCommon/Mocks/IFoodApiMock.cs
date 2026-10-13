using System.Net;

using Orders.Integrations.Hub.IntegrationTests.Payloads;

using WireMock.Server;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public sealed class IFoodApiMock(WireMockServer server) {
    private readonly IWireMockTarget? _target;

    public IFoodApiMock(IWireMockTarget target) : this((WireMockServer)null!) => _target = target;

    private const string Integration = "IFood";
    
    public void StubToken(string token = "ifood-token")
        => Respond(
            "/authentication/v1.0/oauth/token",
            HttpMethod.Post,
            body: PayloadLoader.Load(
                Integration,
                "auth",
                ("accessToken", token)
            )
        );

    public void StubOrderDetails(string orderId = "order-1")
        => Respond(
            "/order/v1.0/orders/*",
            HttpMethod.Get,
            body: PayloadLoader.Load(
                Integration,
                "order-details",
                ("orderId", orderId),
                ("createdAt", DateTime.UtcNow.ToString("O")),
                ("preparationStartDateTime", DateTime.UtcNow.ToString("O")),
                ("deliveryDateTime", DateTime.UtcNow.AddHours(1).ToString("O"))
            )
        );

    public void StubCancellationReasons()
        => Respond(
            "/order/v1.0/orders/*/cancellationReasons",
            HttpMethod.Get,
            body: "[{\"cancelCodeId\":\"0\",\"description\":\"Customer request\"}]"
        );

    public void StubCommandEndpoints() {
        Respond("/order/v1.0/orders/*/confirm", HttpMethod.Post);
        Respond("/order/v1.0/orders/*/preparationStarted", HttpMethod.Post);
        Respond("/order/v1.0/orders/*/readyToPickup", HttpMethod.Post);
        Respond("/order/v1.0/orders/*/dispatch", HttpMethod.Post);
        Respond("/order/v1.0/orders/*/requestCancellation", HttpMethod.Post);
        Respond("/order/v1.0/disputes/*/accept", HttpMethod.Post);
        Respond("/order/v1.0/disputes/*/reject", HttpMethod.Post);
        Respond("/order/v1.0/disputes/*/alternatives/*", HttpMethod.Post);
    }

    public void StubEvidence(string path = "/evidence-1.png")
        => Respond(
            path,
            HttpMethod.Get,
            body: "test-image",
            contentType: "image/png"
        );

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
