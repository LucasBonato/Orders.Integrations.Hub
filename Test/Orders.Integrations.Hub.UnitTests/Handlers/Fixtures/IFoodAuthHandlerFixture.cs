using Microsoft.Extensions.Logging;

using NSubstitute;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Application.Handlers;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.UnitTests.Helpers;

namespace Orders.Integrations.Hub.UnitTests.Handlers.Fixtures;

public sealed class IFoodAuthHandlerFixture : AuthHandlerTestFixture
{
    private ILogger<IFoodAuthMessageHandler> Logger { get; }
    private IIFoodAuthClient AuthClient { get; }
    private ICacheService Cache { get; }
    public override TestHandler InnerHandler { get; }
    public override DelegatingHandler Handler { get; }

    public IFoodAuthHandlerFixture() {
        Logger = CreateLoggerMock<IFoodAuthMessageHandler>();
        AuthClient = Substitute.For<IIFoodAuthClient>();
        Cache = CreateCacheMock();
        InnerHandler = new TestHandler();

        Handler = new IFoodAuthMessageHandler(Logger, AuthClient, Cache) {
            InnerHandler = InnerHandler
        };
    }

    public override IIntegrationContext CreateContext() => CreateDefaultContext();

    public override void AssertAuthHeader(HttpRequestMessage request, string expectedToken)
    {
        Assert.NotNull(request.Headers.Authorization);
        Assert.Equal("Bearer", request.Headers.Authorization.Scheme);
        Assert.Equal(expectedToken, request.Headers.Authorization.Parameter);
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
            .RetrieveToken(Arg.Any<IFoodAuthTokenRequest>())
            .Returns(new IFoodAuthTokenResponse(token, "bearer", (int)expiration.TotalSeconds));
    }

    public override void SetupAuthFailure(Exception exception)
    {
        AuthClient.RetrieveToken(Arg.Any<IFoodAuthTokenRequest>())
            .Returns<Task<IFoodAuthTokenResponse>>(_ => throw exception);
    }
}
