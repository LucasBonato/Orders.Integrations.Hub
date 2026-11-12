using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace Orders.Integrations.Hub.LoadTests.Scenarios.Core;

/// <summary>
/// Covers POST /api/v1/orders-hub/orders/disputes (OrdersHubOrderDisputeEndpoint).
/// Body matches RespondDisputeIntegrationRequest DTO with dispute type rotation.
/// </summary>
public static class OrderDisputeScenario
{
    private static readonly string[] Types = { "ACCEPT", "REJECT" };
    private static int _counter;

    public static ScenarioProps Build(HttpClient httpClient, string baseUrl, LoadProfile profile)
    {
        return Scenario.Create("core_order_dispute", async context =>
            {
                int iteration = Interlocked.Increment(ref _counter);
                string type = Types[iteration % Types.Length];
                string body = $$"""
                    {
                      "disputeId": "dispute-{{iteration}}",
                      "integration": "ifood",
                      "type": "{{type}}",
                      "alternativeId": null,
                      "disputeResponse": {
                        "reason": "Test dispute response",
                        "detailsReason": null,
                        "type": null,
                        "price": null,
                        "additionalTimeInMinutes": null,
                        "additionalTimeReason": null
                      }
                    }
                    """;

                using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                var request = Http.CreateRequest("POST", $"{baseUrl}/api/v1/orders-hub/orders/disputes")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(content);

                var response = await Http.Send(httpClient, request);

                return !response.IsError
                    ? Response.Ok(statusCode: response.StatusCode, sizeBytes: response.SizeBytes)
                    : Response.Fail(statusCode: response.StatusCode);
            })
            .WithLoadSimulations(
                Simulation.KeepConstant(copies: profile.Copies, during: profile.During)
            );
    }
}
