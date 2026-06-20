using System.Net;
using System.Net.Http.Headers;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using NSubstitute;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;
using Orders.Integrations.Hub.Integrations.Food99.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.Integrations.IFood.Application.Handlers;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.UnitTests.Helpers;

namespace Orders.Integrations.Hub.UnitTests.Pipeline;

public class IntegrationPipelineTests
{
    private class FakeServiceProvider(object? service) : IServiceProvider
    {
        public object? GetService(Type serviceType) => service;
    }

    private static IIntegrationContext CreateContext()
    {
        IIntegrationContext? context = Substitute.For<IIntegrationContext>();
        context.TenantId.Returns("tenant-1");
        context.MerchantId.Returns("merchant-1");
        context.Integration.Returns(
            new Integration(
                TenantId: "tenant-1", 
                MerchantId: "merchant-1", 
                ClientId: "client-id", 
                ClientSecret: "client-secret", 
                AutoAccept: false, 
                Mode: IntegrationMode.Distributed
            )
        );
        return context;
    }

    private static FakeServiceProvider CreateServiceProvider(IIntegrationContext context) => new(context);

    private static IHttpContextAccessor CreateHttpContextAccessor(IIntegrationContext context)
    {
        IHttpContextAccessor accessor = Substitute.For<IHttpContextAccessor>();
        accessor.HttpContext.Returns(new DefaultHttpContext { RequestServices = CreateServiceProvider(context) });
        return accessor;
    }

    [Fact]
    public async Task IFoodPipeline_ShouldSetContextAndAuth()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        IIntegrationContext context = CreateContext();
        IHttpContextAccessor httpContextAccessor = CreateHttpContextAccessor(context);
        ICacheService? cache = Substitute.For<ICacheService>();
        cache.GetAsync<string>(Arg.Any<string>()).Returns((string?)null);

        IIFoodAuthClient? authClient = Substitute.For<IIFoodAuthClient>();
        authClient
            .RetrieveToken(Arg.Any<IFoodAuthTokenRequest>())
            .Returns(
                new IFoodAuthTokenResponse(
                    AccessToken: "pipeline-token",
                    Type: "bearer",
                    ExpiresIn: 3600
                )
            );

        TestHandler inner = new();
        IFoodAuthMessageHandler authHandler = new(
            Substitute.For<ILogger<IFoodAuthMessageHandler>>(),
            authClient,
            cache
        );
        IntegrationContextHandler contextHandler = new(httpContextAccessor);

        contextHandler.InnerHandler = authHandler;
        authHandler.InnerHandler = inner;

        // Act
        using HttpMessageInvoker invoker = new(contextHandler);
        using HttpRequestMessage request = new(HttpMethod.Get, "http://localhost/test");
        using HttpResponseMessage response = await invoker.SendAsync(request, ct);

        // Assert
        Assert.Equal(1, inner.CallCount);
        Assert.NotNull(inner.LastRequest);
        HttpRequestHeaders requestHeaders = inner.LastRequest.Headers;
        Assert.NotNull(requestHeaders.Authorization);
        Assert.Equal("pipeline-token", requestHeaders.Authorization.Parameter);
    }

    [Fact]
    public async Task RappiPipeline_ShouldSetContextAndAuth()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        Environment.SetEnvironmentVariable("INTEGRATIONS__RAPPI__CLIENT__AUDIENCE", "test-audience");

        IIntegrationContext context = CreateContext();
        IHttpContextAccessor httpContextAccessor = CreateHttpContextAccessor(context);
        ICacheService? cache = Substitute.For<ICacheService>();
        cache.GetAsync<string>(Arg.Any<string>()).Returns((string?)null);
        
        IRappiAuthClient? authClient = Substitute.For<IRappiAuthClient>();
        
        authClient
            .RetrieveToken(Arg.Any<RappiAuthTokenRequest>())
            .Returns(
                new RappiAuthTokenResponse(
                    AccessToken: "pipeline-token",
                    Scope: "scope",
                    ExpiresIn: 3600,
                    TokenType: "bearer"
                )
            );

        TestHandler inner = new();
        RappiAuthMessageHandler authHandler = new(
            Substitute.For<ILogger<RappiAuthMessageHandler>>(),
            authClient,
            cache
        );
        IntegrationContextHandler contextHandler = new(httpContextAccessor);

        contextHandler.InnerHandler = authHandler;
        authHandler.InnerHandler = inner;

        // Act
        using HttpMessageInvoker invoker = new(contextHandler);
        using HttpRequestMessage request = new(HttpMethod.Get, "http://localhost/test");
        using HttpResponseMessage response = await invoker.SendAsync(request, ct);

        // Assert
        Assert.Equal(1, inner.CallCount);
        Assert.NotNull(inner.LastRequest);
        Assert.True(inner.LastRequest.Headers.TryGetValues("x-authorization", out IEnumerable<string>? values));
        Assert.Equal("Bearer pipeline-token", values.Single());
    }

    [Fact]
    public async Task Food99Pipeline_ShouldSetContextAndInjectTokenIntoBody()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        IIntegrationContext context = CreateContext();
        IHttpContextAccessor httpContextAccessor = CreateHttpContextAccessor(context);
        ICacheService? cache = Substitute.For<ICacheService>();
        cache.GetAsync<string>(Arg.Any<string>()).Returns((string?)null);

        IFood99AuthClient? authClient = Substitute.For<IFood99AuthClient>();
        authClient
            .RetrieveToken(Arg.Any<Food99AuthTokenRequest>())
            .Returns(
                new Food99AuthTokenResponse(
                    Errno: 0,
                    Errmsg: "ok",
                    RequestId: "req-1",
                    Time: 1000,
                    new Food99AuthDataTokenResponse(
                        AppId: 1,
                        AppShopId: "shop-1",
                        AuthToken: "pipeline-token",
                        TokenExpirationTime: 9999999999
                    )
                )
            );

        ICustomJsonSerializer? jsonSerializer = Substitute.For<ICustomJsonSerializer>();
        
        jsonSerializer
            .Deserialize<Food99BaseResponse>(Arg.Any<string>())
            .Returns(
                new Food99BaseResponse(
                    Errno: 0,
                    Errmsg: "ok",
                    RequestId: "req-1",
                    Time: 1000
                )
            );

        const string orderId = "order-123";
        jsonSerializer
            .Deserialize<Food99StatusChangeRequest>(Arg.Any<string>())
            .Returns(
                new Food99StatusChangeRequest(
                    OrderId: orderId,
                    AuthToken: null,
                    ReasonId: null,
                    Reason: null
                )
            );
        jsonSerializer
            .Serialize(Arg.Any<Food99StatusChangeRequest>())
            .Returns(callInfo => {
                Food99StatusChangeRequest? req = callInfo.Arg<Food99StatusChangeRequest>();
                return $$"""{"order_id":"{{req.OrderId}}","auth_token":"{{req.AuthToken}}"}""";
            });

        TestHandler inner = new(new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new StringContent(
                    """{"errno":0,"errmsg":"ok","requestId":"r1","time":1000}""",
                    Encoding.UTF8, 
                    "application/json"
                )
        });
        Food99AuthMessageHandler authHandler = new(
            jsonSerializer,
            Substitute.For<ILogger<Food99AuthMessageHandler>>(),
            authClient,
            cache
        );
        IntegrationContextHandler contextHandler = new(httpContextAccessor);

        contextHandler.InnerHandler = authHandler;
        authHandler.InnerHandler = inner;

        // Act
        using HttpMessageInvoker invoker = new(contextHandler);
        using HttpRequestMessage request = new(HttpMethod.Post, "http://localhost/test");
        const string body = """{"order_id":"order-123"}""";
        request.Content = new StringContent(body, Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await invoker.SendAsync(request, ct);

        // Assert
        Assert.Equal(1, inner.CallCount);
        Assert.NotNull(inner.LastRequest);
        string bodyString = await inner.LastRequest.Content!.ReadAsStringAsync(ct);
        Assert.Contains("\"auth_token\":\"pipeline-token\"", bodyString);
    }

    [Fact]
    public async Task Pipeline_ShouldNotSetAuth_WhenAlreadyAuthenticated()
    {
        // Assert
        CancellationToken ct = TestContext.Current.CancellationToken;
        IIntegrationContext context = CreateContext();
        IHttpContextAccessor httpContextAccessor = CreateHttpContextAccessor(context);
        ICacheService? cache = Substitute.For<ICacheService>();

        IIFoodAuthClient? authClient = Substitute.For<IIFoodAuthClient>();
        TestHandler inner = new();

        IFoodAuthMessageHandler authHandler = new(
            Substitute.For<ILogger<IFoodAuthMessageHandler>>(),
            authClient,
            cache
        );
        IntegrationContextHandler contextHandler = new(httpContextAccessor);

        contextHandler.InnerHandler = authHandler;
        authHandler.InnerHandler = inner;

        // Act
        using HttpMessageInvoker invoker = new(contextHandler);
        using HttpRequestMessage request = new(HttpMethod.Get, "http://localhost/test");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "pre-set");
        using HttpResponseMessage response = await invoker.SendAsync(request, ct);

        Assert.Equal(1, inner.CallCount);
        Assert.NotNull(inner.LastRequest);
        Assert.Equal("pre-set", inner.LastRequest.Headers.Authorization!.Parameter);
        await authClient.DidNotReceiveWithAnyArgs().RetrieveToken(Arg.Any<IFoodAuthTokenRequest>());
    }

    [Fact]
    public async Task Pipeline_ShouldSkipContext_WhenNoHttpContext()
    {
        // Arrange
        CancellationToken ct = TestContext.Current.CancellationToken;
        IHttpContextAccessor? httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns((HttpContext?)null);
        ICacheService? cache = Substitute.For<ICacheService>();
        cache.GetAsync<string>(Arg.Any<string>()).Returns((string?)null);

        IIFoodAuthClient? authClient = Substitute.For<IIFoodAuthClient>();
        authClient
            .RetrieveToken(Arg.Any<IFoodAuthTokenRequest>())
            .Returns(
                new IFoodAuthTokenResponse(
                    AccessToken: "token",
                    Type: "bearer",
                    ExpiresIn: 3600
                )
            );

        TestHandler inner = new();

        IFoodAuthMessageHandler authHandler = new(
            Substitute.For<ILogger<IFoodAuthMessageHandler>>(),
            authClient,
            cache
        );
        IntegrationContextHandler contextHandler = new(httpContextAccessor);

        contextHandler.InnerHandler = authHandler;
        authHandler.InnerHandler = inner;

        // Act
        using HttpMessageInvoker invoker = new(contextHandler);
        using HttpRequestMessage request = new(HttpMethod.Get, "http://localhost/test");
        
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => invoker.SendAsync(request, ct));
    }
}