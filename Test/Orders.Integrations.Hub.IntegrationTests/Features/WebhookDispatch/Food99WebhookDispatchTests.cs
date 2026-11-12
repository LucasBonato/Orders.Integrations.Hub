using System.Net;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures.Collections;
using Orders.Integrations.Hub.IntegrationTests.Payloads;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Helpers;
using Orders.Integrations.Hub.TestCommon.Contracts;
using Orders.Integrations.Hub.TestCommon.Contracts.Food99;
using Orders.Integrations.Hub.TestCommon.Host;

namespace Orders.Integrations.Hub.IntegrationTests.Features.WebhookDispatch;

[Collection(IntegrationTestCollection.Name)]
public sealed class Food99WebhookDispatchTests(
    IntegrationHostFixture fixture
) : IntegrationTestBase(fixture) {
    private const string Route = "/api/v1/orders-hub/food99/webhook";
    private const string Integration = "Food99";
    private readonly IIntegrationContract _contract = Food99Contract.Instance;

    [Fact]
    public async Task PostOrderNew_ShouldReturnSuccess_AndCreateOrderThroughOrdersApi()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        await Host.WireMock.OrdersApi.StubCreateOrderAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, "order-new"),
            Food99Contract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForCreateOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, await Host.WireMock.OrdersApi.RequestCountAsync("/Orders", TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task PostOrderUpdate_ShouldReturnSuccess_AndPatchOrders()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        await Host.WireMock.OrdersApi.StubPatchOrderAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, "order-update"),
            Food99Contract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForPatchOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, await Host.WireMock.OrdersApi.RequestCountAsync("/Orders", TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task PostOrderDispute_ShouldReturnSuccess_AndPatchOrderDispute()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        await Host.WireMock.OrdersApi.StubPatchOrderDisputeAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            Route,
            PayloadLoader.Load(Integration, "order-dispute"),
            Food99Contract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForPatchOrderDisputeAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, await Host.WireMock.OrdersApi.RequestCountAsync("/Orders/dispute", TestContext.Current.CancellationToken));
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
