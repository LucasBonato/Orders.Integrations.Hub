using System.Net;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures.Collections;
using Orders.Integrations.Hub.IntegrationTests.Payloads;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Helpers;
using Orders.Integrations.Hub.TestCommon.Contracts;
using Orders.Integrations.Hub.TestCommon.Contracts.IFood;
using Orders.Integrations.Hub.TestCommon.Host;
using Orders.Integrations.Hub.TestCommon.Mocks;

namespace Orders.Integrations.Hub.IntegrationTests.Features.WebhookDispatch;

[Collection(IntegrationTestCollection.Name)]
public sealed class IFoodWebhookDispatchTests(
    IntegrationHostFixture fixture
) : IntegrationTestBase(fixture) {
    private const string Route = "/api/v1/orders-hub/ifood/webhook";
    private const string Integration = "IFood";
    private readonly IIntegrationContract _contract = IFoodContract.Instance;

    [Fact]
    public async Task PostKeepAlive_ShouldReturnAccepted_WithoutCallingExternalApis()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        
        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, "keepalive"),
            IFoodContract.Instance
        );

        // Assert
        Assert.Equal(202, (int)result.StatusCode);
        Assert.Equal(0, await Host.WireMock.IFoodApi.RequestCountAsync("/", TestContext.Current.CancellationToken));
        Assert.Equal(0, await Host.WireMock.OrdersApi.RequestCountAsync("/Orders", TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task PostPlaced_ShouldReturnAccepted_AndCallPlatformAndOrdersApis()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        await Host.WireMock.IFoodApi.StubTokenAsync(cancellationToken: TestContext.Current.CancellationToken);
        await Host.WireMock.IFoodApi.StubOrderDetailsAsync(cancellationToken: TestContext.Current.CancellationToken);
        await Host.WireMock.OrdersApi.StubCreateOrderAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, "placed", ("orderId", "order-1")),
            IFoodContract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForCreateOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, await Host.WireMock.IFoodApi.RequestCountAsync("/order/v1.0/orders/order-1", TestContext.Current.CancellationToken));
        Assert.Equal(1, await Host.WireMock.OrdersApi.RequestCountAsync("/Orders", TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task PostPlaced_ShouldUseOrderIdFromRawPayload()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        await Host.WireMock.IFoodApi.StubTokenAsync(cancellationToken: TestContext.Current.CancellationToken);
        await Host.WireMock.IFoodApi.StubOrderDetailsAsync("order-999", TestContext.Current.CancellationToken);
        await Host.WireMock.OrdersApi.StubCreateOrderAsync(cancellationToken: TestContext.Current.CancellationToken);
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
        Assert.Equal(1, await Host.WireMock.IFoodApi.RequestCountAsync("/order/v1.0/orders/order-999", TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData("confirmed")]
    [InlineData("cancelled")]
    public async Task PostStatusEvent_ShouldReturnAccepted_AndPatchOrders(string eventName)
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        await Host.WireMock.OrdersApi.StubPatchOrderAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, eventName),
            IFoodContract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForPatchOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, await Host.WireMock.OrdersApi.RequestCountAsync("/Orders", TestContext.Current.CancellationToken));
    }
}
