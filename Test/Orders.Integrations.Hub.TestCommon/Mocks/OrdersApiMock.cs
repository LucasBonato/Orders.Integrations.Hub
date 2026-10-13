using System.Net;

using WireMock.Server;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public sealed class OrdersApiMock(WireMockServer server) {
    private readonly IWireMockTarget? _target;

    public OrdersApiMock(IWireMockTarget target) : this((WireMockServer)null!) => _target = target;

    public void StubCreateOrder(HttpStatusCode statusCode = HttpStatusCode.OK)
        => Respond("/Orders", HttpMethod.Post, statusCode);

    public void StubPatchOrder(HttpStatusCode statusCode = HttpStatusCode.OK)
        => Respond("/Orders", HttpMethod.Patch, statusCode);

    public void StubPatchOrderDispute(HttpStatusCode statusCode = HttpStatusCode.OK)
        => Respond("/Orders/dispute", HttpMethod.Patch, statusCode);

    public Task WaitForCreateOrderAsync(CancellationToken cancellationToken)
        => WireMockApi.WaitForRequestAsync(server, "/Orders", cancellationToken);

    public Task WaitForPatchOrderAsync(CancellationToken cancellationToken)
        => WireMockApi.WaitForRequestAsync(server, "/Orders", cancellationToken);

    public Task WaitForPatchOrderDisputeAsync(CancellationToken cancellationToken)
        => WireMockApi.WaitForRequestAsync(server, "/Orders/dispute", cancellationToken);

    public int RequestCount(string path) => _target?.RequestCount(path) ?? WireMockApi.RequestCount(server, path);
    public string LastRequestBody(string path) => WireMockApi.LastRequestBody(server, path);

    private void Respond(
        string path, 
        HttpMethod method, 
        HttpStatusCode statusCode,
        string body = "{}",
        string contentType = "application/json"
    )
        => (_target is null
            ? new Action(() => WireMockApi.Respond(server, path, method, statusCode, body, contentType))
            : new Action(() => _target.Respond(path, method, statusCode, body, contentType)))();
}
