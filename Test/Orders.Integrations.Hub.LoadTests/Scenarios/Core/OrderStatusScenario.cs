using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace Orders.Integrations.Hub.LoadTests.Scenarios.Core;

/// <summary>
/// Covers PATCH /api/v1/orders-hub/orders/status (OrdersHubOrderStatusEndpoint).
/// Body matches ChangeOrderStatusRequest DTO with status rotation.
/// </summary>
public static class OrderStatusScenario
{
    private static readonly string[] IFoodStatuses = ["CONFIRMED", "PREPARING", "READY_FOR_PICKUP", "CANCELLED"];
    private static readonly string[] RappiStatuses = ["CONFIRMED", "READY_FOR_PICKUP", "CANCELLED"];
    private static readonly string[] Food99Statuses = ["CONFIRMED", "READY_FOR_PICKUP", "DELIVERED", "CANCELLED"];
    private static int _counter;

    public static ScenarioProps Smoke(HttpClient httpClient, string baseUrl, LoadProfile profile) =>
        Build(httpClient, baseUrl, "core_order_status_smoke", "ifood", IFoodStatuses, "503", profile);

    public static ScenarioProps IFoodOutbound(HttpClient httpClient, string baseUrl) =>
        Build(httpClient, baseUrl, "core_order_status_ifood", "ifood", IFoodStatuses, "503", LoadTestSettings.PartnerOutboundSteadyState);

    public static ScenarioProps RappiOutbound(HttpClient httpClient, string baseUrl) =>
        Build(httpClient, baseUrl, "core_order_status_rappi", "rappi", RappiStatuses, "0", LoadTestSettings.PartnerOutboundSteadyState);

    public static ScenarioProps Food99Outbound(HttpClient httpClient, string baseUrl) =>
        Build(httpClient, baseUrl, "core_order_status_food99", "food99", Food99Statuses, "CustomerCancelled", LoadTestSettings.PartnerOutboundSteadyState);

    private static ScenarioProps Build(
        HttpClient httpClient,
        string baseUrl,
        string name,
        string integration,
        string[] statuses,
        string cancellationReason,
        LoadProfile profile)
    {
        return Scenario.Create(name, async context =>
        {
            int iteration = Interlocked.Increment(ref _counter);
            string status = statuses[iteration % statuses.Length];
            string cancellationReasonJson = status == "CANCELLED"
                ? System.Text.Json.JsonSerializer.Serialize(cancellationReason)
                : "null";
            string body = $$"""
                {
                  "orderId": "order-{{iteration}}",
                  "externalId": "ext-order-{{iteration}}",
                  "merchantId": "merchant-1",
                  "status": "{{status}}",
                  "integration": "{{integration}}",
                  "cancellationReason": {{cancellationReasonJson}},
                  "cancellationMetadata": null
                }
                """;

            using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            var request = Http.CreateRequest("PATCH", $"{baseUrl}/api/v1/orders-hub/orders/status")
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
