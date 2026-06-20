using System.Net;

namespace Orders.Integrations.Hub.UnitTests.Helpers;

public class TestHandler : HttpMessageHandler
{
    public HttpRequestMessage? LastRequest { get; private set; }
    public int CallCount { get; private set; }
    public Func<HttpRequestMessage, HttpResponseMessage>? ResponseFactory { get; set; }

    public TestHandler(HttpResponseMessage? response = null)
    {
        if (response is not null)
            ResponseFactory = _ => response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        CallCount++;
        HttpResponseMessage response = ResponseFactory?.Invoke(request) ?? new HttpResponseMessage(HttpStatusCode.OK);
        return Task.FromResult(response);
    }
}
