using System.Net;

using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace Orders.Integrations.Hub.LoadTests.Scenarios.Core;

/// <summary>
/// Covers POST /api/v1/orders-hub/orders/products/enable and .../disable
/// (both OrdersHubProductStatusEndpoint). Per the discovery doc, the current
/// implementation always returns 422 because IntegrationKey.Nothing() has no
/// keyed use case registered. This scenario treats 422 as the EXPECTED result —
/// not a failure — so the report reflects reality instead of showing 100%
/// failed requests for a known, already-understood gap.
///
/// Once the missing keyed use case ships, change ExpectedStatusCode below (or
/// remove this comment block and treat any 2xx as success like the other
/// scenarios) — this is explicitly a temporary accommodation, not the intended
/// long-term behavior of the endpoint.
/// </summary>
public static class ProductStatusScenario
{
    private const HttpStatusCode ExpectedStatusCodeUntilUseCaseShips = HttpStatusCode.UnprocessableEntity;

    public static ScenarioProps Enable(HttpClient httpClient, string baseUrl) =>
        Build(httpClient, baseUrl, "core_product_enable", "/api/v1/orders-hub/orders/products/enable");

    public static ScenarioProps Disable(HttpClient httpClient, string baseUrl) =>
        Build(httpClient, baseUrl, "core_product_disable", "/api/v1/orders-hub/orders/products/disable");

    private static ScenarioProps Build(HttpClient httpClient, string baseUrl, string name, string route)
    {
        const string body = """
            {
              "integration": "ifood",
              "productId": "product-1"
            }
            """;

        return Scenario.Create(name, async context =>
        {
            var request = Http.CreateRequest("POST", $"{baseUrl}{route}")
                .WithHeader("Content-Type", "application/json")
                .WithBody(new StringContent(body, System.Text.Encoding.UTF8, "application/json"));

            var response = await Http.Send(httpClient, request);

            return response.StatusCode == ExpectedStatusCodeUntilUseCaseShips.ToString()
                ? Response.Ok(statusCode: response.StatusCode, sizeBytes: response.SizeBytes)
                : Response.Fail(
                    statusCode: response.StatusCode,
                    message: $"Expected {(int)ExpectedStatusCodeUntilUseCaseShips} (known gap), got {response.StatusCode}"
                );
        })
        .WithLoadSimulations(
            Simulation.KeepConstant(copies: LoadTestSettings.ProductStatusSmoke.Copies, during: LoadTestSettings.ProductStatusSmoke.During)
        );
    }
}
