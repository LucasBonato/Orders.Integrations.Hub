using System.Net;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

/// <summary>Shared WireMock operations for each external API fake.</summary>
public abstract class WireMockApiMock(
    WireMockAdminClient adminClient
) {
    protected Task RespondAsync(
        string path,
        HttpMethod method,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string body = "{}",
        string contentType = "application/json",
        CancellationToken cancellationToken = default
    ) => adminClient.RegisterStubAsync(path, method.Method, (int)statusCode, body, contentType, cancellationToken);

    public Task<int> RequestCountAsync(string path, CancellationToken cancellationToken = default)
        => adminClient.RequestCountAsync(path, cancellationToken);

    public Task<string?> LastRequestBodyAsync(string path, CancellationToken cancellationToken = default)
        => adminClient.LastRequestBodyAsync(path, cancellationToken);

    protected Task WaitForRequestAsync(string path, CancellationToken cancellationToken, TimeSpan? timeout = null)
        => adminClient.WaitForRequestAsync(path, cancellationToken, timeout);
}
