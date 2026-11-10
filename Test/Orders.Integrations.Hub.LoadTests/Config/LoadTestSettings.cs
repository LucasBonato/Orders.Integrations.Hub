using System.Globalization;

namespace Orders.Integrations.Hub.LoadTests;

/// <summary>
/// The single source of truth for every tweakable load parameter. Every scenario
/// pulls its <c>Simulation.*</c> arguments from here — never inline a raw number in
/// a scenario file. All values are overridable via environment variable so CI and
/// local runs can tune load without touching code.
/// </summary>
public sealed record LoadProfile(int Copies, TimeSpan During);

public sealed record RampProfile(int Rate, TimeSpan Interval, TimeSpan During);

public static class LoadTestSettings
{
    // ---- Webhooks (default; high priority per discovery doc) ----------------

    public static LoadProfile WebhookSteadyState { get; } = new(
        Copies: GetInt("LOADTEST_WEBHOOK_COPIES", 3),
        During: GetDuration("LOADTEST_WEBHOOK_DURATION", TimeSpan.FromSeconds(30)));

    public static RampProfile WebhookBurst { get; } = new(
        Rate: GetInt("LOADTEST_WEBHOOK_BURST_RATE", 10),
        Interval: GetDuration("LOADTEST_WEBHOOK_BURST_INTERVAL", TimeSpan.FromSeconds(1)),
        During: GetDuration("LOADTEST_WEBHOOK_BURST_DURATION", TimeSpan.FromSeconds(10)));

    // Rappi ping is a lightweight liveness-style call partners send frequently —
    // modeled separately since its traffic shape (constant trickle) differs from
    // order-event webhooks (bursty).
    public static LoadProfile RappiPing { get; } = new(
        Copies: GetInt("LOADTEST_RAPPI_PING_COPIES", 2),
        During: GetDuration("LOADTEST_RAPPI_PING_DURATION", TimeSpan.FromSeconds(30)));

    // ---- Core routes: default "smoke" level (route + validation path only) ---

    public static LoadProfile CoreSmoke { get; } = new(
        Copies: GetInt("LOADTEST_CORE_COPIES", 3),
        During: GetDuration("LOADTEST_CORE_DURATION", TimeSpan.FromSeconds(30)));

    // ---- Core routes: opt-in, heavier — exercises the partner-outbound path ---
    // Structurally safe regardless of opt-in status: every integration base URL is
    // always pointed at the isolated WireMock container (see OutboundHostGuard).
    // Opt-in here is about deliberate load intensity, not network safety.

    public static LoadProfile PartnerOutboundSteadyState { get; } = new(
        Copies: GetInt("LOADTEST_OUTBOUND_COPIES", 5),
        During: GetDuration("LOADTEST_OUTBOUND_DURATION", TimeSpan.FromSeconds(30)));

    // ---- Stress profiles (used with --stress flag) ---------------------------

    public static LoadProfile StressWebhookSteadyState { get; } = new(
        Copies: GetInt("LOADTEST_STRESS_WEBHOOK_COPIES", 10),
        During: GetDuration("LOADTEST_STRESS_WEBHOOK_DURATION", TimeSpan.FromMinutes(2)));

    public static RampProfile StressWebhookBurst { get; } = new(
        Rate: GetInt("LOADTEST_STRESS_WEBHOOK_BURST_RATE", 30),
        Interval: GetDuration("LOADTEST_STRESS_WEBHOOK_BURST_INTERVAL", TimeSpan.FromSeconds(1)),
        During: GetDuration("LOADTEST_STRESS_WEBHOOK_BURST_DURATION", TimeSpan.FromSeconds(20)));

    public static LoadProfile StressCoreSmoke { get; } = new(
        Copies: GetInt("LOADTEST_STRESS_CORE_COPIES", 10),
        During: GetDuration("LOADTEST_STRESS_CORE_DURATION", TimeSpan.FromMinutes(2)));

    // ---- Product status endpoints (enable/disable) ----------------------------
    // Current implementation returns 422 for every call because IntegrationKey.Nothing()
    // has no keyed use case registered. Keep this profile light — it's tracking a
    // known gap, not measuring real throughput. Remove the 422-as-expected handling
    // in the scenario once the use case ships.

    public static LoadProfile ProductStatusSmoke { get; } = new(
        Copies: GetInt("LOADTEST_PRODUCT_STATUS_COPIES", 5),
        During: GetDuration("LOADTEST_PRODUCT_STATUS_DURATION", TimeSpan.FromSeconds(30)));

    // ---- Summary report thresholds -------------------------------------------

    public static double MaxErrorRate { get; } =
        GetDouble("LOADTEST_MAX_ERROR_RATE", 0.05); // 5% max error rate

    public static double MaxP95ResponseTimeMs { get; } =
        GetDouble("LOADTEST_MAX_P95_RESPONSE_TIME_MS", 5000); // 5 seconds

    public static string ReportsRoot { get; } = GetReportsRoot();

    private static int GetInt(string envVar, int fallback)
    {
        string? raw = Environment.GetEnvironmentVariable(envVar);
        if (raw is null)
            return fallback;
        if (!int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value) || value <= 0)
            throw new InvalidOperationException($"{envVar} must be a positive integer; received '{raw}'.");
        return value;
    }

    private static double GetDouble(string envVar, double fallback)
    {
        string? raw = Environment.GetEnvironmentVariable(envVar);
        if (raw is null)
            return fallback;
        if (!double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
            || !double.IsFinite(value))
            throw new InvalidOperationException($"{envVar} must be a finite number; received '{raw}'.");
        return value;
    }

    private static TimeSpan GetDuration(string envVar, TimeSpan fallback)
    {
        string? raw = Environment.GetEnvironmentVariable(envVar);
        if (raw is null)
            return fallback;
        if (!TimeSpan.TryParse(raw, CultureInfo.InvariantCulture, out TimeSpan value) || value <= TimeSpan.Zero)
            throw new InvalidOperationException($"{envVar} must be a positive duration; received '{raw}'.");
        return value;
    }

    private static string GetReportsRoot()
    {
        string? configuredPath = Environment.GetEnvironmentVariable("LOADTEST_REPORTS_DIR");
        if (!string.IsNullOrWhiteSpace(configuredPath))
            return Path.GetFullPath(configuredPath);

        DirectoryInfo? directory = new(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Orders.Integrations.Hub.slnx"))
                || Directory.Exists(Path.Combine(directory.FullName, ".git")))
                return Path.Combine(directory.FullName, "reports");
            directory = directory.Parent;
        }

        return Path.Combine(Environment.CurrentDirectory, "reports");
    }

    public static void Validate()
    {
        if (MaxErrorRate is < 0 or > 1)
            throw new InvalidOperationException("LOADTEST_MAX_ERROR_RATE must be between 0 and 1 (for example, 0.05 for 5%).");
        if (MaxP95ResponseTimeMs <= 0)
            throw new InvalidOperationException("LOADTEST_MAX_P95_RESPONSE_TIME_MS must be positive.");
    }
}
