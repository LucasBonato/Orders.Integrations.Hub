using System.Net;

using Orders.Integrations.Hub.IntegrationTests.Contracts;
using Orders.Integrations.Hub.IntegrationTests.Contracts.IFood;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Mocks;
using Orders.Integrations.Hub.IntegrationTests.Payloads;
using Orders.Integrations.Hub.IntegrationTests.Requests;

namespace Orders.Integrations.Hub.IntegrationTests.Entrypoints.Webhooks.IFood;

public sealed class IFoodWebhookDispatchTests : IntegrationTestBase
{
    private const string Route = "/api/v1/orders-hub/ifood/webhook";
    private const string Integration = "IFood";
    private readonly IIntegrationContract _contract = new IFoodContract();

    [Fact]
    public async Task PostKeepAlive_ShouldReturnAccepted_WithoutCallingExternalApis()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        
        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, "keepalive"),
            IFoodContract.Instance);

        // Assert
        Assert.Equal(202, (int)result.StatusCode);
        Assert.Empty(Host.WireMock.IFood.LogEntries);
        Assert.Empty(Host.WireMock.Orders.LogEntries);
    }

    [Fact]
    public async Task PostPlaced_ShouldReturnAccepted_AndCallPlatformAndOrdersApis()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        Host.WireMock.IFoodApi.StubToken();
        Host.WireMock.IFoodApi.StubOrderDetails();
        Host.WireMock.OrdersApi.StubCreateOrder();

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, "placed", ("orderId", "order-1")),
            IFoodContract.Instance
        );

        // Assert
        Assert.Equal(202, (int)result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForCreateOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, WireMockApi.RequestCount(Host.WireMock.IFood, "/order/v1.0/orders/order-1"));
        Assert.Equal(1, Host.WireMock.OrdersApi.RequestCount("/Orders"));
    }

    [Fact]
    public async Task PostPlaced_ShouldUseOrderIdFromRawPayload()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        Host.WireMock.IFoodApi.StubToken();
        Host.WireMock.IFoodApi.StubOrderDetails("order-999");
        Host.WireMock.OrdersApi.StubCreateOrder();
        string payload = PayloadLoader.Load(
            Integration,
            "placed",
            ("orderId", "order-999")
        );
        
        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route, 
            payload, 
            IFoodContract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForCreateOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, WireMockApi.RequestCount(Host.WireMock.IFood, "/order/v1.0/orders/order-999"));
    }

    [Theory]
    [InlineData("confirmed")]
    [InlineData("cancelled")]
    public async Task PostStatusEvent_ShouldReturnAccepted_AndPatchOrders(string eventName)
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        Host.WireMock.OrdersApi.StubPatchOrder();

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, eventName),
            IFoodContract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForPatchOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, Host.WireMock.OrdersApi.RequestCount("/Orders"));
    }
}
