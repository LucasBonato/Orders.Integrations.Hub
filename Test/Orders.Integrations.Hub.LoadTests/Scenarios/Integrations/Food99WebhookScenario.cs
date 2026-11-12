using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;

using Orders.Integrations.Hub.LoadTests.Payloads;

namespace Orders.Integrations.Hub.LoadTests.Scenarios.Integrations;

/// <summary> Covers POST /api/v1/orders-hub/food99/webhook (Food99WebhookEndpoint). </summary>
public static class Food99WebhookScenario
{
    public static ScenarioProps Build(HttpClient httpClient, string baseUrl, LoadProfile steadyState, RampProfile burst)
    {
        var feed = WebhookPayloadFeeds.Food99();

        return Scenario.Create("food99_webhook", async context =>
            {
                WebhookPayload payload = feed.GetNextItem(context.ScenarioInfo);
                IReadOnlyDictionary<string, string> headers = WebhookSignatures.HeadersFor("FOOD99", payload.RawJson);

                var request = Http.CreateRequest("POST", $"{baseUrl}/api/v1/orders-hub/food99/webhook")
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
