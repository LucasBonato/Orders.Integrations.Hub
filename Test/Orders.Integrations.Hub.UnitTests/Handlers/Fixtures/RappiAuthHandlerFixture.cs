using Microsoft.Extensions.Logging;

using NSubstitute;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.UnitTests.Helpers;

namespace Orders.Integrations.Hub.UnitTests.Handlers.Fixtures;

public sealed class RappiAuthHandlerFixture : AuthHandlerTestFixture {
    private ILogger<RappiAuthMessageHandler> Logger { get; }
    private IRappiAuthClient AuthClient { get; }
    private ICacheService Cache { get; }
    public override TestHandler InnerHandler { get; }
    public override DelegatingHandler Handler { get; }

    public RappiAuthHandlerFixture() {
        Environment.SetEnvironmentVariable("INTEGRATIONS__RAPPI__CLIENT__AUDIENCE", "test-audience");

        Logger = CreateLoggerMock<RappiAuthMessageHandler>();
        Cache = CreateCacheMock();

        AuthClient = Substitute.For<IRappiAuthClient>();
        InnerHandler = new TestHandler();

        Handler = new RappiAuthMessageHandler(Logger, AuthClient, Cache) {
            InnerHandler = InnerHandler
        };
    }

    public override IIntegrationContext CreateContext() => CreateDefaultContext();

    public override void AssertAuthHeader(HttpRequestMessage request, string expectedToken)
    {
        Assert.True(request.Headers.TryGetValues("x-authorization", out IEnumerable<string>? values));
        Assert.Equal($"Bearer {expectedToken}", values.Single());
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
            .RetrieveToken(Arg.Any<RappiAuthTokenRequest>())
            .Returns(
                new RappiAuthTokenResponse(
                    AccessToken: token, 
                    Scope: "scope",
                    ExpiresIn: (int)expiration.TotalSeconds, 
                    TokenType: "bearer"
                )
            );
    }

    public override void SetupAuthFailure(Exception exception)
    {
        AuthClient
            .RetrieveToken(Arg.Any<RappiAuthTokenRequest>())
            .Returns<Task<RappiAuthTokenResponse>>(_ => throw exception);
    }
}
