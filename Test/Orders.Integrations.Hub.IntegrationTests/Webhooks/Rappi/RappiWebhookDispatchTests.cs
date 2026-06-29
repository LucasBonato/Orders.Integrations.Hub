using System.Text.Json;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure;

namespace Orders.Integrations.Hub.IntegrationTests.Webhooks.Rappi;

public sealed class RappiWebhookDispatchTests : IAsyncLifetime
{
    private IntegrationTestHost _host = null!;
    private const string CreateRoute = "/api/v1/orders-hub/rappi/webhook";
    private const string PingRoute = "/api/v1/orders-hub/rappi/webhook/ping";
    private const string CancelRoute = "/api/v1/orders-hub/rappi/webhook/cancel";
    private const string OtherRoute = "/api/v1/orders-hub/rappi/webhook/other";

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
    public async Task PostCreate_ShouldReturn201_AndDispatchOrder()
    {
        string payload = PayloadLoader.LoadRaw("Rappi", "create");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForRappi(_host.Http)
            .WithRoute(CreateRoute)
            .WithPayload(payload)
            .WithSignature("test-rappi-secret")
            .PostAsync();

        Assert.Equal(201, (int)result.Response.StatusCode);
    }

    [Fact]
    public async Task PostPing_ShouldReturn200_WithPongBody()
    {
        string payload = PayloadLoader.LoadRaw("Rappi", "ping");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForRappi(_host.Http)
            .WithRoute(PingRoute)
            .WithPayload(payload)
            .WithSignature("test-rappi-secret")
            .PostAsync();

        Assert.Equal(200, (int)result.Response.StatusCode);

        string body = await result.Response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        using JsonDocument doc = JsonDocument.Parse(body);
        Assert.Equal("Ok", doc.RootElement.GetProperty("status").GetString());
        Assert.Equal("Store on", doc.RootElement.GetProperty("description").GetString());
    }

    [Fact]
    public async Task PostCancel_ShouldReturn202_AndUpdateOrder()
    {
        string payload = PayloadLoader.LoadRaw("Rappi", "cancel");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForRappi(_host.Http)
            .WithRoute(CancelRoute)
            .WithPayload(payload)
            .WithSignature("test-rappi-secret")
            .PostAsync();

        Assert.Equal(202, (int)result.Response.StatusCode);
    }

    [Fact]
    public async Task PostOther_ShouldReturn202_AndUpdateOrder()
    {
        string payload = PayloadLoader.LoadRaw("Rappi", "other");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForRappi(_host.Http)
            .WithRoute(OtherRoute)
            .WithPayload(payload)
            .WithSignature("test-rappi-secret")
            .PostAsync();

        Assert.Equal(202, (int)result.Response.StatusCode);
    }
}
