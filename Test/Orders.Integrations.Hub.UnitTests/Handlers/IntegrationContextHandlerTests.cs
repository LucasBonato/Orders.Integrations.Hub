using System.Net;

using Microsoft.AspNetCore.Http;

using NSubstitute;

using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Common.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;
using Orders.Integrations.Hub.UnitTests.Helpers;

namespace Orders.Integrations.Hub.UnitTests.Handlers;

public class IntegrationContextHandlerTests
{
    private class FakeServiceProvider(object? service) : IServiceProvider
    {
        public object? GetService(Type serviceType) => service;
    }

    private static IntegrationContext CreateContext() 
        => new() {
            TenantId = "tenant-1",
            MerchantId = "merchant-1",
            Integration = new Integration("tenant-1", "merchant-1", "client-id", "client-secret", false, IntegrationMode.Distributed)
        };

    [Fact]
    public async Task SendAsync_ShouldSetContext_WhenHttpContextHasContext()
    {
        // Arrange
        IntegrationContext context = CreateContext();
        IHttpContextAccessor? httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        FakeServiceProvider serviceProvider = new(context);
        httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { RequestServices = serviceProvider });

        TestHandler inner = new();
        IntegrationContextHandler handler = new(httpContextAccessor) { InnerHandler = inner };

        // Act
        using HttpMessageInvoker invoker = new(handler);
        using HttpRequestMessage request = new(HttpMethod.Get, "http://localhost/test");

        await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        IIntegrationContext retrieved = inner.LastRequest!.GetIntegrationContext();
        Assert.Same(context, retrieved);
    }

    [Fact]
    public async Task SendAsync_ShouldNotSetContext_WhenHttpContextIsNull()
    {
        // Arrange
        IHttpContextAccessor? httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns((HttpContext?)null);

        TestHandler inner = new();
        IntegrationContextHandler handler = new(httpContextAccessor) { InnerHandler = inner };

        // Act
        using HttpMessageInvoker invoker = new (handler);
        using HttpRequestMessage request = new (HttpMethod.Get, "http://localhost/test");

        // Assert
        await invoker.SendAsync(request, CancellationToken.None);

        Assert.Throws<InvalidOperationException>(() => inner.LastRequest!.GetIntegrationContext());
    }

    [Fact]
    public async Task SendAsync_ShouldNotSetContext_WhenServiceNotRegistered()
    {
        // Arrange
        IHttpContextAccessor? httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        FakeServiceProvider serviceProvider = new(null);
        httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { RequestServices = serviceProvider });

        TestHandler inner = new();
        IntegrationContextHandler handler = new(httpContextAccessor) { InnerHandler = inner };
        
        // Act
        using HttpMessageInvoker invoker = new(handler);
        using HttpRequestMessage request = new(HttpMethod.Get, "http://localhost/test");

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => invoker.SendAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task SendAsync_ShouldDelegateToInnerHandler()
    {
        // Arrange
        IntegrationContext context = CreateContext();
        IHttpContextAccessor? httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        FakeServiceProvider serviceProvider = new(context);
        httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { RequestServices = serviceProvider });

        TestHandler inner = new(new HttpResponseMessage(HttpStatusCode.Created));
        IntegrationContextHandler handler = new(httpContextAccessor) { InnerHandler = inner };

        // Act
        using HttpMessageInvoker invoker = new(handler);
        using HttpRequestMessage request = new(HttpMethod.Get, "http://localhost/test");

        using HttpResponseMessage response = await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(1, inner.CallCount);
    }
}
