using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;

namespace Orders.Integrations.Hub.UnitTests.Extensions;

public class HttpRequestMessageExtensionTests
{
    private static IntegrationContext CreateContext() =>
        new() {
            TenantId = "tenant-1",
            MerchantId = "merchant-1",
            Integration = new Integration(
                TenantId: "tenant-1", 
                MerchantId: "merchant-1", 
                ClientId: "client-id", 
                ClientSecret: "client-secret", 
                AutoAccept: false, 
                Mode: IntegrationMode.Distributed
            )
        };

    [Fact]
    public void SetIntegrationContext_ShouldStoreContext()
    {
        HttpRequestMessage request = new();
        IntegrationContext context = CreateContext();

        request.SetIntegrationContext(context);

        IIntegrationContext retrieved = request.GetIntegrationContext();
        Assert.Same(context, retrieved);
    }

    [Fact]
    public void GetIntegrationContext_ShouldThrow_WhenNotSet()
    {
        HttpRequestMessage request = new();

        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(request.GetIntegrationContext);
        Assert.Contains("IntegrationContext was not set", ex.Message);
    }

    [Fact]
    public void SetIntegrationContext_ShouldOverwritePreviousContext()
    {
        HttpRequestMessage request = new();
        IntegrationContext context1 = CreateContext();
        IntegrationContext context2 = CreateContext();

        request.SetIntegrationContext(context1);
        request.SetIntegrationContext(context2);

        IIntegrationContext retrieved = request.GetIntegrationContext();
        Assert.Same(context2, retrieved);
    }
}
