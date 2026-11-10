using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace Orders.Integrations.Hub.LoadTests.Scenarios.Core;

/// <summary>
/// Covers GET /api/v1/orders-hub/orders/cancellation-reasons
/// (OrdersHubOrderCancellationReasonEndpoint). Each integration value resolves
/// its keyed use case and exercises that partner's outbound path against WireMock.
/// </summary>
public static class CancellationReasonsScenario
{
    public static ScenarioProps Build(HttpClient httpClient, string baseUrl, string integration, LoadProfile profile)
    {
        return Scenario.Create(
                name: $"core_cancellation_reasons_{integration}", 
                run: async _ => {
                    HttpRequestMessage request = Http.CreateRequest(
                        "GET",
                        $"{baseUrl}/api/v1/orders-hub/orders/cancellation-reasons?externalOrderId=order-1&integration={integration}"
                    );

                    Response<HttpResponseMessage> response = await Http.Send(httpClient, request);

                    return !response.IsError
                        ? Response.Ok(statusCode: response.StatusCode, sizeBytes: response.SizeBytes)
                        : Response.Fail(statusCode: response.StatusCode);
                }
            )
            .WithLoadSimulations(
                Simulation.KeepConstant(copies: profile.Copies, during: profile.During)
            );
    }
}
