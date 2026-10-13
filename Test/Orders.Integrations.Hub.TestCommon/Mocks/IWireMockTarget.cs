using System.Net;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public interface IWireMockTarget
{
    Uri Url { get; }

    void Respond(
        string path,
        HttpMethod httpMethod,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string body = "{}",
        string contentType = "application/json"
    );

    int RequestCount(string path);

    void ResetRequests();
}

public sealed class InProcessWireMockTarget(WireMock.Server.WireMockServer server) : IWireMockTarget
{
    public Uri Url => new(server.Url ?? throw new InvalidOperationException("WireMock server has no URL."));

    public void Respond(
        string path,
        HttpMethod httpMethod,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string body = "{}",
        string contentType = "application/json"
    ) => WireMockApi.Respond(server, path, httpMethod, statusCode, body, contentType);

    public int RequestCount(string path) => WireMockApi.RequestCount(server, path);

    public void ResetRequests() => server.ResetLogEntries();
}
