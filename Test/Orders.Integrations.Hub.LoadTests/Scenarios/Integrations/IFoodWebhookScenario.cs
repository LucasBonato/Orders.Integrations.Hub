using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;

using Orders.Integrations.Hub.LoadTests.Payloads;

namespace Orders.Integrations.Hub.LoadTests.Scenarios.Integrations;

/// <summary> Covers POST /api/v1/orders-hub/ifood/webhook (IFoodWebhookEndpoint). </summary>
public static class IFoodWebhookScenario
{
    public static ScenarioProps Build(HttpClient httpClient, string baseUrl, LoadProfile steadyState, RampProfile burst)
    {
        var feed = WebhookPayloadFeeds.IFood();

        return Scenario.Create("ifood_webhook", async context =>
            {
                WebhookPayload payload = feed.GetNextItem(context.ScenarioInfo);
                IReadOnlyDictionary<string, string> headers = WebhookSignatures.HeadersFor("IFOOD", payload.RawJson);

                var request = Http.CreateRequest("POST", $"{baseUrl}/api/v1/orders-hub/ifood/webhook")
                    .WithHeader("Content-Type", "application/json");

                foreach (var header in headers)
                    request = request.WithHeader(header.Key, header.Value);

                request = request.WithBody(new StringContent(payload.RawJson, System.Text.Encoding.UTF8, "application/json"));

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
