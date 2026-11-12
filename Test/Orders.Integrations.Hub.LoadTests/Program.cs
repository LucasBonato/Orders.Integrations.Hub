using NBomber.Contracts;
using NBomber.CSharp;
using Orders.Integrations.Hub.LoadTests;
using Orders.Integrations.Hub.LoadTests.Infrastructure;
using Orders.Integrations.Hub.LoadTests.Scenarios.Core;
using Orders.Integrations.Hub.LoadTests.Scenarios.Integrations;
using Orders.Integrations.Hub.TestCommon.Host;

bool includeOutbound = args.Contains("--include-outbound");
bool stress = args.Contains("--stress");
bool hostOnly = args.Contains("--host-only");
string[] runnerArgs = args.Where(arg => arg is not "--include-outbound" and not "--stress" and not "--host-only").ToArray();

LoadTestSettings.Validate();
await using LoadTestHost host = await LoadTestHost.StartAsync(IntegrationTestData.GetIntegrationContext());

if (hostOnly)
{
    Console.WriteLine($"[host-only] Kestrel listening on {host.BaseUrl} — ready for external load testers (e.g. k6).");
    Console.WriteLine("[host-only] Press Ctrl+C to stop.");
    await Task.Delay(Timeout.Infinite);
    await host.DisposeAsync();
    return;
}

using HttpClient httpClient = new() { BaseAddress = new Uri(host.BaseUrl) };

// ---- Pick profiles based on mode ----
LoadProfile webhookProfile = stress ? LoadTestSettings.StressWebhookSteadyState : LoadTestSettings.WebhookSteadyState;
RampProfile webhookBurstProfile = stress ? LoadTestSettings.StressWebhookBurst : LoadTestSettings.WebhookBurst;
LoadProfile coreProfile = stress ? LoadTestSettings.StressCoreSmoke : LoadTestSettings.CoreSmoke;

if (stress)
    Console.WriteLine("--stress: running with higher load profiles.");

// ---- Default scenarios: every webhook endpoint + Core coverage ----
List<ScenarioProps> scenarios =
[
    IFoodWebhookScenario.Build(httpClient, host.BaseUrl, webhookProfile, webhookBurstProfile),
    Food99WebhookScenario.Build(httpClient, host.BaseUrl, webhookProfile, webhookBurstProfile),
    RappiWebhookScenarios.Create(httpClient, host.BaseUrl, webhookProfile, webhookBurstProfile),
    RappiWebhookScenarios.Cancel(httpClient, host.BaseUrl, webhookProfile, webhookBurstProfile),
    RappiWebhookScenarios.Other(httpClient, host.BaseUrl, webhookProfile, webhookBurstProfile),
    RappiWebhookScenarios.Ping(httpClient, host.BaseUrl),

    CancellationReasonsScenario.Build(httpClient, host.BaseUrl, integration: "rappi", coreProfile),
    CancellationReasonsScenario.Build(httpClient, host.BaseUrl, integration: "food99", coreProfile),
    CancellationReasonsScenario.Build(httpClient, host.BaseUrl, integration: "ifood", coreProfile),
    OrderDisputeScenario.Build(httpClient, host.BaseUrl, coreProfile),
    OrderStatusScenario.Smoke(httpClient, host.BaseUrl, coreProfile),
];

// Cancellation-reasons and IFood dispute/status scenarios already exercise real
// provider use cases against WireMock. Add remaining integration variants only
// when explicitly requested.
if (includeOutbound)
{
    Console.WriteLine("--include-outbound: registering deeper partner-outbound scenarios.");
    scenarios.Add(OrderStatusScenario.RappiOutbound(httpClient, host.BaseUrl));
    scenarios.Add(OrderStatusScenario.Food99Outbound(httpClient, host.BaseUrl));
}

string runDirectory = Path.Join(LoadTestSettings.ReportsRoot,
    $"nbomber-{DateTimeOffset.UtcNow:yyyyMMdd-HHmmss-fff}-{Guid.NewGuid():N}");
Directory.CreateDirectory(runDirectory);
object runMetadata = new
{
    StartedAtUtc = DateTimeOffset.UtcNow,
    Commit = Environment.GetEnvironmentVariable("GITHUB_SHA")
        ?? Environment.GetEnvironmentVariable("BUILD_SOURCEVERSION"),
    Mode = stress ? "stress" : "default",
    IncludeOutbound = includeOutbound,
    Arguments = runnerArgs,
    Profiles = new
    {
        Webhook = webhookProfile,
        WebhookBurst = webhookBurstProfile,
        Core = coreProfile,
        RappiPing = LoadTestSettings.RappiPing,
        PartnerOutbound = includeOutbound ? LoadTestSettings.PartnerOutboundSteadyState : null,
        ProductStatus = LoadTestSettings.ProductStatusSmoke
    },
    Thresholds = new
    {
        MaxErrorRate = LoadTestSettings.MaxErrorRate,
        MaxP95ResponseTimeMs = LoadTestSettings.MaxP95ResponseTimeMs
    }
};
File.WriteAllText(Path.Join(runDirectory, "loadtest-metadata.json"),
    System.Text.Json.JsonSerializer.Serialize(runMetadata, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

NBomberRunner
    .RegisterScenarios([.. scenarios])
    .WithReportFolder(runDirectory)
    .Run(runnerArgs);

SummaryReporter.Generate(runDirectory);
if (SummaryReporter.Passed != true)
    Environment.ExitCode = 1;
