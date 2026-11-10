using System.Net;
using System.Text.Json;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures.Collections;
using Orders.Integrations.Hub.IntegrationTests.Payloads;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Helpers;
using Orders.Integrations.Hub.TestCommon.Contracts;
using Orders.Integrations.Hub.TestCommon.Contracts.Rappi;
using Orders.Integrations.Hub.TestCommon.Host;

namespace Orders.Integrations.Hub.IntegrationTests.Features.WebhookDispatch;

[Collection(IntegrationTestCollection.Name)]
public sealed class RappiWebhookDispatchTests(
    IntegrationHostFixture fixture
) : IntegrationTestBase(fixture) {
    private const string CreateRoute = "/api/v1/orders-hub/rappi/webhook";
    private const string PingRoute = "/api/v1/orders-hub/rappi/webhook/ping";
    private const string CancelRoute = "/api/v1/orders-hub/rappi/webhook/cancel";
    private const string OtherRoute = "/api/v1/orders-hub/rappi/webhook/other";
    private const string Integration = "Rappi";
    private readonly IIntegrationContract _contract = RappiContract.Instance;

    [Fact]
    public async Task PostCreate_ShouldReturnCreated_AndCreateOrderThroughOrdersApi()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        await Host.WireMock.OrdersApi.StubCreateOrderAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            CreateRoute,
            PayloadLoader.Load(Integration, "create"),
            RappiContract.Instance);

        // Assert
        Assert.Equal(201, (int)result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForCreateOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, await Host.WireMock.OrdersApi.RequestCountAsync("/Orders", TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task PostPing_ShouldReturnStoreHealthResponse()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        
        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            PingRoute,
            PayloadLoader.Load(Integration, "ping"),
            RappiContract.Instance
        );

        // Assert
        Assert.Equal(200, (int)result.StatusCode);
        using JsonDocument body = JsonDocument.Parse(
            await result.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
        Assert.Equal("Ok", body.RootElement.GetProperty("status").GetString());
        Assert.Equal("Store on", body.RootElement.GetProperty("description").GetString());
    }

    [Theory]
    [InlineData("cancel")]
    [InlineData("other")]
    public async Task PostOrderEvent_ShouldReturnAccepted_AndPatchOrders(string eventName)
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(_contract));
        await Host.WireMock.OrdersApi.StubPatchOrderAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            eventName == "cancel" ? CancelRoute : OtherRoute,
            PayloadLoader.Load(Integration, eventName),
            RappiContract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForPatchOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, await Host.WireMock.OrdersApi.RequestCountAsync("/Orders", TestContext.Current.CancellationToken));
    }
}
