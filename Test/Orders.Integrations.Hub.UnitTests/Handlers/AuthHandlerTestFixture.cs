using Microsoft.Extensions.Logging;

using NSubstitute;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;
using Orders.Integrations.Hub.UnitTests.Helpers;

using Xunit.Sdk;

namespace Orders.Integrations.Hub.UnitTests.Handlers;

public abstract class AuthHandlerTestFixture : IXunitSerializable
{
    public string Name { get; set; } = string.Empty;
    public abstract TestHandler InnerHandler { get; }
    public abstract DelegatingHandler Handler { get; }
    public abstract IIntegrationContext CreateContext();
    public abstract void AssertAuthHeader(HttpRequestMessage request, string expectedToken);
    public abstract void SetupCacheMiss();
    public abstract void SetupCacheHit(string token);
    public abstract void SetupAuthSuccess(string token, TimeSpan expiration);
    public abstract void SetupAuthFailure(Exception exception);

    private static Integration CreateDefaultIntegration() 
        => new(
            TenantId: "tenant-1", 
            MerchantId: "merchant-1", 
            ClientId: "client-id", 
            ClientSecret: "client-secret", 
            AutoAccept: false, 
            Mode: IntegrationMode.Distributed
        );

    protected static IIntegrationContext CreateDefaultContext() {
        IIntegrationContext? context = Substitute.For<IIntegrationContext>();
        context.TenantId.Returns("tenant-1");
        context.MerchantId.Returns("merchant-1");
        context.Integration.Returns(CreateDefaultIntegration());
        return context;
    }

    protected static ICacheService CreateCacheMock() {
        ICacheService? cache = Substitute.For<ICacheService>();
        cache.GetAsync<string>(Arg.Any<string>()).Returns((string?)null);
        return cache;
    }

    protected static ILogger<T> CreateLoggerMock<T>() => Substitute.For<ILogger<T>>();

    public static HttpRequestMessage CreateRequest(IIntegrationContext context) {
        HttpRequestMessage request = new(HttpMethod.Get, "http://localhost/test");
        request.SetIntegrationContext(context);
        return request;
    }

    public void Deserialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(Name), Name);
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        Name = info.GetValue<string>(nameof(Name))?? string.Empty;
    }
}
