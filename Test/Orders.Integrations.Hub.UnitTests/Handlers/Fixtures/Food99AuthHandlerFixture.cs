using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;

using Microsoft.Extensions.Logging;

using NSubstitute;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.UnitTests.Helpers;

namespace Orders.Integrations.Hub.UnitTests.Handlers.Fixtures;

public sealed class Food99AuthHandlerFixture : AuthHandlerTestFixture
{
    private ILogger<Food99AuthMessageHandler> Logger { get; }
    private IFood99AuthClient AuthClient { get; }
    private ICacheService Cache { get; }
    private ICustomJsonSerializer JsonSerializer { get; }
    public override TestHandler InnerHandler { get; }
    public override DelegatingHandler Handler { get; }

    public Food99AuthHandlerFixture()
    {
        Logger = CreateLoggerMock<Food99AuthMessageHandler>();
        AuthClient = Substitute.For<IFood99AuthClient>();
        Cache = CreateCacheMock();
        JsonSerializer = Substitute.For<ICustomJsonSerializer>();
        InnerHandler = new TestHandler();

        JsonSerializer
            .Deserialize<Food99BaseResponse>(Arg.Any<string>())
            .Returns(new Food99BaseResponse(0, "ok", "req-1", 1000));

        Handler = new Food99AuthMessageHandler(JsonSerializer, Logger, AuthClient, Cache) {
            InnerHandler = InnerHandler
        };
    }

    public override IIntegrationContext CreateContext() => CreateDefaultContext();

    public override void AssertAuthHeader(HttpRequestMessage request, string expectedToken)
    {
        if (request.Method == HttpMethod.Get) {
            Assert.NotNull(request.RequestUri);
            NameValueCollection query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            Assert.Equal(expectedToken, query["auth_token"]);
        }
        else
        {
            Assert.NotNull(request.Content);
            string body = request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Assert.Contains($"\"auth_token\":\"{expectedToken}\"", body);
        }
    }

    public override void SetupCacheMiss()
    {
        Cache.GetAsync<string>(Arg.Any<string>()).Returns((string?)null);
    }

    public override void SetupCacheHit(string token)
    {
        Cache.GetAsync<string>(Arg.Any<string>()).Returns(token);
    }

    public override void SetupAuthSuccess(string token, TimeSpan expiration)
    {
        AuthClient
            .RetrieveToken(Arg.Any<Food99AuthTokenRequest>())
            .Returns(
                new Food99AuthTokenResponse(
                    Errno: 0, 
                    Errmsg: "ok", 
                    RequestId: "req-1", 
                    Time: 1000,
                    Data: new Food99AuthDataTokenResponse(
                        AppId: 1, 
                        AppShopId: "shop-1", 
                        AuthToken: token, 
                        TokenExpirationTime: 9999999999
                    )
                )
            );
    }

    public override void SetupAuthFailure(Exception exception)
    {
        AuthClient
            .RetrieveToken(Arg.Any<Food99AuthTokenRequest>())
            .Returns<Task<Food99AuthTokenResponse>>(_ => throw exception);
    }

    public void SetupTokenExpiredResponse()
    {
        HttpResponseMessage expiredResponse = new(HttpStatusCode.OK) {
            Content = new StringContent(
                """{"errno":10102,"errmsg":"expired","requestId":"r1","time":1000}""",
                Encoding.UTF8,
                "application/json"
            )
        };
        InnerHandler.ResponseFactory = _ => expiredResponse;
    }

    public void SetupTokenOkResponse() {
        HttpResponseMessage okResponse = new(HttpStatusCode.OK) {
            Content = new StringContent(
                """{"errno":0,"errmsg":"ok","requestId":"r1","time":1000}""",
                Encoding.UTF8,
                "application/json"
            )
        };
        InnerHandler.ResponseFactory = _ => okResponse;
    }

    public void SetupJsonSerializerForBodyInjection(string orderId) {
        Food99StatusChangeRequest original = new(orderId, null, null, null);
        JsonSerializer
            .Deserialize<Food99StatusChangeRequest>(Arg.Any<string>())
            .Returns(original);

        JsonSerializer
            .Serialize(Arg.Any<Food99StatusChangeRequest>())
            .Returns(callInfo => {
                Food99StatusChangeRequest? req = callInfo.Arg<Food99StatusChangeRequest>();
                return $$"""{"order_id":"{{req.OrderId}}","auth_token":"{{req.AuthToken}}"}""";
            });
    }
}
