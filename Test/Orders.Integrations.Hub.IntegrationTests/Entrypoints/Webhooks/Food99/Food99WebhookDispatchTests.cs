using System.Net;

using Orders.Integrations.Hub.IntegrationTests.Contracts;
using Orders.Integrations.Hub.IntegrationTests.Contracts.Food99;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.IntegrationTests.Payloads;
using Orders.Integrations.Hub.IntegrationTests.Requests;

namespace Orders.Integrations.Hub.IntegrationTests.Entrypoints.Webhooks.Food99;

public sealed class Food99WebhookDispatchTests : IntegrationTestBase
{
    private const string Route = "/api/v1/orders-hub/food99/webhook";
    private const string Integration = "Food99";
    private readonly IIntegrationContract _contract = new Food99Contract();

    [Fact]
    public async Task PostOrderNew_ShouldReturnSuccess_AndCreateOrderThroughOrdersApi()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        Host.WireMock.OrdersApi.StubCreateOrder();

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, "order-new"),
            Food99Contract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForCreateOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, Host.WireMock.OrdersApi.RequestCount("/Orders"));
    }

    [Fact]
    public async Task PostOrderUpdate_ShouldReturnSuccess_AndPatchOrders()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        Host.WireMock.OrdersApi.StubPatchOrder();

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, "order-update"),
            Food99Contract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForPatchOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, Host.WireMock.OrdersApi.RequestCount("/Orders"));
    }

    [Fact]
    public async Task PostOrderDispute_ShouldReturnSuccess_AndPatchOrderDispute()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        Host.WireMock.OrdersApi.StubPatchOrderDispute();

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, "order-dispute"),
            Food99Contract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForPatchOrderDisputeAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, Host.WireMock.OrdersApi.RequestCount("/Orders/dispute"));
    }

    [Fact]
    public async Task Post_ShouldReturnFailure_WhenWebhookTypeIsUnknown()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        string payload = PayloadLoader.Load(
            Integration,
            "order-unknown"
        );

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route, 
            payload, 
            Food99Contract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
    }
}
