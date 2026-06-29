using NSubstitute;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure;

namespace Orders.Integrations.Hub.IntegrationTests.Webhooks.IFood;

public sealed class IFoodWebhookDispatchTests : IAsyncLifetime
{
    private IntegrationTestHost _host = null!;
    private const string Route = "/api/v1/orders-hub/ifood/webhook";

    public ValueTask InitializeAsync()
    {
        _host = IntegrationTestHost.Create();
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _host.DisposeAsync();
    }

    [Fact]
    public async Task PostKeepAlive_ShouldReturn202_AndNotCallExternalClients()
    {
        string payload = PayloadLoader.LoadRaw("IFood", "keepalive");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForIFood(_host.Http)
            .WithRoute(Route)
            .WithPayload(payload)
            .WithSignature("test-secret")
            .PostAsync();

        Assert.Equal(202, (int)result.Response.StatusCode);

        await _host.IFoodClient.DidNotReceiveWithAnyArgs().GetOrderDetails(null!);
    }

    [Fact]
    public async Task PostPlaced_ShouldReturn202_AndFetchOrderDetails()
    {
        string payload = PayloadLoader.LoadRaw("IFood", "placed");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForIFood(_host.Http)
            .WithRoute(Route)
            .WithPayload(payload)
            .WithSignature("test-secret")
            .PostAsync();

        Assert.Equal(202, (int)result.Response.StatusCode);

        await _host.IFoodClient.Received(1).GetOrderDetails("order-456");
    }

    [Fact]
    public async Task PostConfirmed_ShouldReturn202_WithoutFetchingOrderDetails()
    {
        string payload = PayloadLoader.LoadRaw("IFood", "confirmed");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForIFood(_host.Http)
            .WithRoute(Route)
            .WithPayload(payload)
            .WithSignature("test-secret")
            .PostAsync();

        Assert.Equal(202, (int)result.Response.StatusCode);

        await _host.IFoodClient.DidNotReceiveWithAnyArgs().GetOrderDetails(null!);
    }

    [Fact]
    public async Task PostCancelled_ShouldReturn202_WithoutFetchingOrderDetails()
    {
        string payload = PayloadLoader.LoadRaw("IFood", "cancelled");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForIFood(_host.Http)
            .WithRoute(Route)
            .WithPayload(payload)
            .WithSignature("test-secret")
            .PostAsync();

        Assert.Equal(202, (int)result.Response.StatusCode);

        await _host.IFoodClient.DidNotReceiveWithAnyArgs().GetOrderDetails(null!);
    }
}