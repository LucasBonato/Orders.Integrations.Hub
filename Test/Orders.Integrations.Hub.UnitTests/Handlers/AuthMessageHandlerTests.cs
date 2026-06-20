using System.Net;
using System.Net.Http.Headers;

using Orders.Integrations.Hub.Integrations.Common.Contracts;

namespace Orders.Integrations.Hub.UnitTests.Handlers;

public class AuthMessageHandlerTests
{
    [Theory]
    [ClassData(typeof(AuthHandlerFixtureProvider))]
    public async Task SendAsync_ShouldSkipAuth_WhenAuthorizationHeaderAlreadySet(AuthHandlerTestFixture fixture)
    {
        // Arrange
        IIntegrationContext context = fixture.CreateContext();
        HttpRequestMessage request = AuthHandlerTestFixture.CreateRequest(context);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "existing-token");

        fixture.SetupCacheMiss();
        
        // Act
        using HttpMessageInvoker invoker = new(fixture.Handler);
        using HttpResponseMessage response = await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(1, fixture.InnerHandler.CallCount);
        HttpRequestMessage inner = fixture.InnerHandler.LastRequest!;
        Assert.NotNull(inner.Headers.Authorization);
        Assert.Equal("Bearer", inner.Headers.Authorization.Scheme);
        Assert.Equal("existing-token", inner.Headers.Authorization.Parameter);
    }

    [Theory]
    [ClassData(typeof(AuthHandlerFixtureProvider))]
    public async Task SendAsync_ShouldThrow_WhenContextMissing(AuthHandlerTestFixture fixture)
    {
        // Arrange
        HttpRequestMessage request = new(HttpMethod.Get, "http://localhost/test");

        fixture.SetupCacheMiss();

        // Act
        using HttpMessageInvoker invoker = new(fixture.Handler);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => invoker.SendAsync(request, CancellationToken.None));
    }

    [Theory]
    [ClassData(typeof(AuthHandlerFixtureProvider))]
    public async Task SendAsync_ShouldCallAuthClient_OnCacheMiss(AuthHandlerTestFixture fixture)
    {
        // Arrange
        IIntegrationContext context = fixture.CreateContext();
        HttpRequestMessage request = AuthHandlerTestFixture.CreateRequest(context);

        fixture.SetupCacheMiss();
        fixture.SetupAuthSuccess("new-token", TimeSpan.FromHours(1));

        // Act
        using HttpMessageInvoker invoker = new(fixture.Handler);
        using HttpResponseMessage response = await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(1, fixture.InnerHandler.CallCount);
    }

    [Theory]
    [ClassData(typeof(AuthHandlerFixtureProvider))]
    public async Task SendAsync_ShouldSetAuthHeader_WhenTokenRetrieved(AuthHandlerTestFixture fixture)
    {
        // Arrange
        IIntegrationContext context = fixture.CreateContext();
        HttpRequestMessage request = AuthHandlerTestFixture.CreateRequest(context);

        fixture.SetupCacheMiss();
        fixture.SetupAuthSuccess("fresh-token", TimeSpan.FromHours(1));

        // Act
        using HttpMessageInvoker invoker = new(fixture.Handler);
        using HttpResponseMessage response = await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        fixture.AssertAuthHeader(fixture.InnerHandler.LastRequest!, "fresh-token");
    }

    [Theory]
    [ClassData(typeof(AuthHandlerFixtureProvider))]
    public async Task SendAsync_ShouldUseCachedToken_OnCacheHit(AuthHandlerTestFixture fixture)
    {
        // Arrange
        IIntegrationContext context = fixture.CreateContext();
        HttpRequestMessage request = AuthHandlerTestFixture.CreateRequest(context);

        fixture.SetupCacheHit("cached-token");

        // Act
        using HttpMessageInvoker invoker = new(fixture.Handler);
        using HttpResponseMessage response = await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(1, fixture.InnerHandler.CallCount);
        fixture.AssertAuthHeader(fixture.InnerHandler.LastRequest!, "cached-token");
    }

    [Theory]
    [ClassData(typeof(AuthHandlerFixtureProvider))]
    public async Task SendAsync_ShouldPropagateException_WhenAuthClientFails(AuthHandlerTestFixture fixture)
    {
        // Arrange
        IIntegrationContext context = fixture.CreateContext();
        HttpRequestMessage request = AuthHandlerTestFixture.CreateRequest(context);

        fixture.SetupCacheMiss();
        fixture.SetupAuthFailure(new HttpRequestException("Auth failed"));

        // Act
        using HttpMessageInvoker invoker = new(fixture.Handler);

        // Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => invoker.SendAsync(request, CancellationToken.None));
    }

    [Theory]
    [ClassData(typeof(AuthHandlerFixtureProvider))]
    public async Task SendAsync_ShouldNotCallAuthClient_WhenCached(AuthHandlerTestFixture fixture)
    {
        // Arrange
        IIntegrationContext context = fixture.CreateContext();
        HttpRequestMessage request = AuthHandlerTestFixture.CreateRequest(context);

        fixture.SetupCacheHit("valid-cached-token");

        // Act
        using HttpMessageInvoker invoker = new(fixture.Handler);
        using HttpResponseMessage response = await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(1, fixture.InnerHandler.CallCount);
    }

    [Theory]
    [ClassData(typeof(AuthHandlerFixtureProvider))]
    public async Task SendAsync_ShouldPassResponse_WhenAuthSucceeds(AuthHandlerTestFixture fixture)
    {
        // Assert
        IIntegrationContext context = fixture.CreateContext();
        HttpRequestMessage request = AuthHandlerTestFixture.CreateRequest(context);

        fixture.SetupCacheMiss();
        fixture.SetupAuthSuccess("token", TimeSpan.FromHours(1));
        fixture.InnerHandler.ResponseFactory = _ => new HttpResponseMessage(HttpStatusCode.Created);

        // Act
        using HttpMessageInvoker invoker = new(fixture.Handler);
        using HttpResponseMessage response = await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
