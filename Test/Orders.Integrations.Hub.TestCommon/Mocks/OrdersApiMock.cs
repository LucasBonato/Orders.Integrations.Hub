using System.Net;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public sealed class OrdersApiMock(WireMockAdminClient adminClient) : WireMockApiMock(adminClient) {
    public Task StubCreateOrderAsync(HttpStatusCode statusCode = HttpStatusCode.OK, CancellationToken cancellationToken = default)
        => RespondAsync("/Orders", HttpMethod.Post, statusCode, cancellationToken: cancellationToken);

    public Task StubPatchOrderAsync(HttpStatusCode statusCode = HttpStatusCode.OK, CancellationToken cancellationToken = default)
        => RespondAsync("/Orders", HttpMethod.Patch, statusCode, cancellationToken: cancellationToken);

    public Task StubPatchOrderDisputeAsync(HttpStatusCode statusCode = HttpStatusCode.OK, CancellationToken cancellationToken = default)
        => RespondAsync("/Orders/dispute", HttpMethod.Patch, statusCode, cancellationToken: cancellationToken);

    public Task WaitForCreateOrderAsync(CancellationToken cancellationToken)
        => WaitForRequestAsync("/Orders", cancellationToken);

    public Task WaitForPatchOrderAsync(CancellationToken cancellationToken)
        => WaitForRequestAsync("/Orders", cancellationToken);

    public Task WaitForPatchOrderDisputeAsync(CancellationToken cancellationToken)
        => WaitForRequestAsync("/Orders/dispute", cancellationToken);
}
