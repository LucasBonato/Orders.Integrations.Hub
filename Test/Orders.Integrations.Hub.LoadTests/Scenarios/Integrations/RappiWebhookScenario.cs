using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;

using Orders.Integrations.Hub.LoadTests.Payloads;

namespace Orders.Integrations.Hub.LoadTests.Scenarios.Integrations;

/// <summary>
/// Covers the four Rappi webhook routes: RappiWebhookCreateOrderEndpoint,
/// RappiWebhookCancelOrderEndpoint, RappiWebhookPatchOrderEndpoint (grouped here
/// as "Other"), and RappiWebhookPingEndpoint.
/// </summary>
public static class RappiWebhookScenarios
{
    private const string RoutePrefix = "/api/v1/orders-hub/rappi/webhook";

    public static ScenarioProps Create(HttpClient httpClient, string baseUrl, LoadProfile steadyState, RampProfile burst) =>
        Build(httpClient, baseUrl, "rappi_webhook_create", RoutePrefix, WebhookPayloadFeeds.RappiCreate(),
            steadyState, burst);

    public static ScenarioProps Cancel(HttpClient httpClient, string baseUrl, LoadProfile steadyState, RampProfile burst) =>
        Build(httpClient, baseUrl, "rappi_webhook_cancel", $"{RoutePrefix}/cancel", WebhookPayloadFeeds.RappiCancel(),
            steadyState, burst);

    public static ScenarioProps Other(HttpClient httpClient, string baseUrl, LoadProfile steadyState, RampProfile burst) =>
        Build(httpClient, baseUrl, "rappi_webhook_other", $"{RoutePrefix}/other", WebhookPayloadFeeds.RappiOther(),
            steadyState, burst);

    public static ScenarioProps Ping(HttpClient httpClient, string baseUrl)
    {
        var feed = WebhookPayloadFeeds.RappiPing();

        return Scenario.Create("rappi_webhook_ping", async context =>
        {
            WebhookPayload payload = feed.GetNextItem(context.ScenarioInfo);
            IReadOnlyDictionary<string, string> headers = WebhookSignatures.HeadersFor("RAPPI", payload.RawJson);

            var request = Http.CreateRequest("POST", $"{baseUrl}{RoutePrefix}/ping")
                .WithHeader("Content-Type", "application/json");

            foreach (var header in headers)
                request = request.WithHeader(header.Key, header.Value);

            using var content = new StringContent(payload.RawJson, System.Text.Encoding.UTF8, "application/json");
            request = request.WithBody(content);

            var response = await Http.Send(httpClient, request);

            return !response.IsError
                ? Response.Ok(statusCode: response.StatusCode, sizeBytes: response.SizeBytes)
                : Response.Fail(statusCode: response.StatusCode);
        })
        .WithLoadSimulations(
            Simulation.Inject(rate: LoadTestSettings.RappiPing.Copies, interval: TimeSpan.FromSeconds(1), during: LoadTestSettings.RappiPing.During)
        );
    }

    private static ScenarioProps Build(
        HttpClient httpClient,
        string baseUrl,
        string name,
        string route,
        NBomber.Data.IDataFeed<WebhookPayload> feed,
        LoadProfile steadyState,
        RampProfile burst)
    {
        return Scenario.Create(name, async context =>
        {
            WebhookPayload payload = feed.GetNextItem(context.ScenarioInfo);
            IReadOnlyDictionary<string, string> headers = WebhookSignatures.HeadersFor("RAPPI", payload.RawJson);

            var request = Http.CreateRequest("POST", $"{baseUrl}{route}")
                .WithHeader("Content-Type", "application/json");

            foreach (var header in headers)
                request = request.WithHeader(header.Key, header.Value);

            using var content = new StringContent(payload.RawJson, System.Text.Encoding.UTF8, "application/json");
            request = request.WithBody(content);

            var response = await Http.Send(httpClient, request);

            return !response.IsError
                ? Response.Ok(statusCode: response.StatusCode, sizeBytes: response.SizeBytes)
                : Response.Fail(statusCode: response.StatusCode, message: payload.EventType);
        })
        .WithLoadSimulations(
            Simulation.Inject(rate: steadyState.Copies, interval: TimeSpan.FromSeconds(1), during: steadyState.During),
            Simulation.RampingInject(rate: burst.Rate, interval: burst.Interval, during: burst.During)
        );
    }
}
