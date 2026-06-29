using Orders.Integrations.Hub.IntegrationTests.Infrastructure;

namespace Orders.Integrations.Hub.IntegrationTests.Webhooks.IFood;

public sealed class IFoodSignatureValidationTests : IAsyncLifetime
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
    public async Task Post_ShouldReturn401_WhenSignatureHeaderIsMissing()
    {
        string payload = PayloadLoader.LoadRaw("IFood", "keepalive");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForIFood(_host.Http)
            .WithRoute(Route)
            .WithPayload(payload)
            .WithoutSignature()
            .PostAsync();

        Assert.Equal(401, (int)result.Response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturn401_WhenSignatureIsInvalid()
    {
        string payload = PayloadLoader.LoadRaw("IFood", "keepalive");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForIFood(_host.Http)
            .WithRoute(Route)
            .WithPayload(payload)
            .WithSignature("wrong-secret")
            .PostAsync();

        Assert.Equal(401, (int)result.Response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturn202_WhenSignatureIsValid()
    {
        string payload = PayloadLoader.LoadRaw("IFood", "keepalive");

        ScenarioResult result = await WebhookScenarioBuilder
            .ForIFood(_host.Http)
            .WithRoute(Route)
            .WithPayload(payload)
            .WithSignature("test-secret")
            .PostAsync();

        Assert.Equal(202, (int)result.Response.StatusCode);
    }
}