using System.Globalization;

namespace Orders.Integrations.Hub.LoadTests;

/// <summary>Writes a thresholded summary for the current NBomber run.</summary>
public static class SummaryReporter
{
    public static bool? Passed { get; private set; }

    public static void Generate(string reportDirectory)
    {
            try
            {
                string[] csvFiles = Directory.GetFiles(reportDirectory, "nbomber_report_*.csv");
                if (csvFiles.Length == 0)
                    throw new FileNotFoundException($"NBomber produced no CSV report in '{reportDirectory}'.");

                string csvPath = csvFiles.OrderByDescending(File.GetLastWriteTimeUtc).First();
                List<string[]> rows = ParseCsv(File.ReadAllText(csvPath));
                if (rows.Count < 2)
                    throw new InvalidDataException($"NBomber CSV report '{csvPath}' has no scenario rows.");

                string[] header = rows[0];
                Dictionary<string, int> headerColumns = header
                    .Select((name, index) => (name, index))
                    .ToDictionary(item => item.name, item => item.index, StringComparer.OrdinalIgnoreCase);

                Dictionary<string, string[]> columnAliases = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["scenario"] = ["scenario"],
                    ["step_name"] = ["step_name"],
                    ["duration"] = ["duration"],
                    ["ok"] = ["ok", "ok_request_count"],
                    ["failed"] = ["failed", "fail_request_count"],
                    ["ok_rps"] = ["ok_rps", "ok_request_rps"],
                    ["ok_min"] = ["ok_min", "ok_latency_min"],
                    ["ok_mean"] = ["ok_mean", "ok_latency_mean"],
                    ["ok_50_percent"] = ["ok_50_percent", "ok_latency_50"],
                    ["ok_95_percent"] = ["ok_95_percent", "ok_latency_95"],
                    ["ok_99_percent"] = ["ok_99_percent", "ok_latency_99"],
                    ["ok_max"] = ["ok_max", "ok_latency_max"],
                    ["ok_std_dev"] = ["ok_std_dev", "ok_latency_std_dev"],
                    ["fail_rps"] = ["fail_rps", "fail_request_rps"],
                    ["fail_mean"] = ["fail_mean", "fail_latency_mean"],
                    ["fail_95_percent"] = ["fail_95_percent", "fail_latency_95"],
                    ["data_transfer_all_mb"] = ["data_transfer_all_mb", "ok_data_all_mb"],
                    ["data_transfer_bytes_per_sec"] = ["data_transfer_bytes_per_sec", "ok_data_bytes_per_sec"]
                };
                Dictionary<string, int> columns = columnAliases
                    .Select(pair => (pair.Key, Index: pair.Value
                        .FirstOrDefault(headerColumns.ContainsKey) is string actualName
                            ? headerColumns[actualName]
                            : -1))
                    .Where(item => item.Index >= 0)
                    .ToDictionary(item => item.Key, item => item.Index, StringComparer.OrdinalIgnoreCase);

                string[] required = ["scenario", "step_name", "duration", "ok", "failed",
                    "ok_rps", "ok_min", "ok_mean", "ok_50_percent", "ok_95_percent", "ok_99_percent", "ok_max",
                    "ok_std_dev", "fail_rps", "fail_mean", "fail_95_percent", "data_transfer_all_mb",
                    "data_transfer_bytes_per_sec"];
                string[] missing = required.Where(name => !columns.ContainsKey(name)).ToArray();
                if (missing.Length > 0)
                    throw new InvalidDataException($"NBomber CSV is missing required columns: {string.Join(", ", missing)}.");

                List<ScenarioResult> scenarios = rows.Skip(1)
                    .Where(row => Get(row, columns, "step_name") == "global information")
                    .Select(row => new ScenarioResult(
                        Get(row, columns, "scenario"),
                        ParseLong(row, columns, "ok") + ParseLong(row, columns, "failed"),
                        ParseLong(row, columns, "ok"),
                        ParseLong(row, columns, "failed"),
                        ParseDouble(row, columns, "ok_rps"),
                        ParseDouble(row, columns, "ok_min"),
                        ParseDouble(row, columns, "ok_mean"),
                        ParseDouble(row, columns, "ok_50_percent"),
                        ParseDouble(row, columns, "ok_95_percent"),
                        ParseDouble(row, columns, "ok_99_percent"),
                        ParseDouble(row, columns, "ok_max"),
                        ParseDouble(row, columns, "ok_std_dev"),
                        ParseDouble(row, columns, "fail_rps"),
                        ParseDouble(row, columns, "fail_mean"),
                        ParseDouble(row, columns, "fail_95_percent"),
                        ParseDouble(row, columns, "data_transfer_all_mb"),
                        ParseDouble(row, columns, "data_transfer_bytes_per_sec"),
                        Get(row, columns, "duration")))
                    .ToList();

                if (scenarios.Count == 0)
                    throw new InvalidDataException("NBomber CSV contains no 'global information' scenario rows.");

                Passed = true;
                List<string> output = [
                    new string('=', 80),
                    "NBomber Load Test Summary",
                    $"Generated: {DateTimeOffset.UtcNow:yyyy-MM-dd HH:mm:ss} UTC",
                    $"Run folder: {Path.GetFullPath(reportDirectory)}",
                    new string('=', 80),
                    string.Empty
                ];

                long totalOk = 0;
                long totalFailed = 0;
                double totalOkRps = 0;

                foreach (ScenarioResult scenario in scenarios)
                {
                    double errorRate = scenario.Requests == 0
                        ? 100
                        : scenario.Failed * 100d / scenario.Requests;
                    bool scenarioPass = scenario.Requests > 0
                        && errorRate <= LoadTestSettings.MaxErrorRate * 100
                        && scenario.P95 <= LoadTestSettings.MaxP95ResponseTimeMs;
                    Passed &= scenarioPass;
                    totalOk += scenario.Ok;
                    totalFailed += scenario.Failed;
                    totalOkRps += scenario.OkRps;

                    output.Add(new string('-', 80));
                    output.Add($"Scenario: {scenario.Name}");
                    output.Add($"  Duration: {scenario.Duration} | Requests: {scenario.Requests:N0} | OK: {scenario.Ok:N0} | Failed: {scenario.Failed:N0}");
                    output.Add($"  Error rate: {errorRate.ToString("F2", CultureInfo.InvariantCulture)}% | OK RPS: {scenario.OkRps.ToString("F2", CultureInfo.InvariantCulture)}");
                    output.Add($"  OK latency ms: min {scenario.Min.ToString("F2", CultureInfo.InvariantCulture)} | mean {scenario.Mean.ToString("F2", CultureInfo.InvariantCulture)} | p50 {scenario.P50.ToString("F2", CultureInfo.InvariantCulture)} | p95 {scenario.P95.ToString("F2", CultureInfo.InvariantCulture)} | p99 {scenario.P99.ToString("F2", CultureInfo.InvariantCulture)} | max {scenario.Max.ToString("F2", CultureInfo.InvariantCulture)} | stddev {scenario.StdDev.ToString("F2", CultureInfo.InvariantCulture)}");
                    output.Add($"  Failed RPS: {scenario.FailedRps.ToString("F2", CultureInfo.InvariantCulture)} | Failed mean/p95 ms: {scenario.FailedMean.ToString("F2", CultureInfo.InvariantCulture)}/{scenario.FailedP95.ToString("F2", CultureInfo.InvariantCulture)}");
                    output.Add($"  Data: {scenario.DataTransferredMb.ToString("F3", CultureInfo.InvariantCulture)} MB | {scenario.TransferBytesPerSecond.ToString("F2", CultureInfo.InvariantCulture)} bytes/s");
                    output.Add($"  Threshold: error <= {(LoadTestSettings.MaxErrorRate * 100).ToString("F2", CultureInfo.InvariantCulture)}%, p95 <= {LoadTestSettings.MaxP95ResponseTimeMs.ToString("F2", CultureInfo.InvariantCulture)} ms | Verdict: {(scenarioPass ? "PASS" : "FAIL")}");
                    output.Add(string.Empty);
                }

                long totalRequests = totalOk + totalFailed;
                double overallErrorRate = totalRequests == 0 ? 100 : totalFailed * 100d / totalRequests;
                output.Add(new string('=', 80));
                output.Add("Overall Summary");
                output.Add($"Requests: {totalRequests:N0} | OK: {totalOk:N0} | Failed: {totalFailed:N0} | Error rate: {overallErrorRate.ToString("F2", CultureInfo.InvariantCulture)}%");
                output.Add($"Sum of scenario OK RPS: {totalOkRps.ToString("F2", CultureInfo.InvariantCulture)}");
                output.Add($"Overall verdict: {(Passed == true ? "PASS" : "FAIL")}");
                output.Add(new string('=', 80));

                string summary = string.Join(Environment.NewLine, output);
                Console.WriteLine(summary);
                File.WriteAllText(Path.Combine(reportDirectory, "nbomber-summary.txt"), summary);
            }
            catch (Exception exception)
            {
                Passed = false;
                Console.Error.WriteLine($"NBomber summary generation failed: {exception}");
            }

    }

    private static string Get(string[] row, IReadOnlyDictionary<string, int> columns, string name)
    {
        int index = columns[name];
        if (index >= row.Length)
            throw new InvalidDataException($"NBomber CSV row is missing value for '{name}'.");
        return row[index];
    }

    private static long ParseLong(string[] row, IReadOnlyDictionary<string, int> columns, string name)
        => long.TryParse(Get(row, columns, name), NumberStyles.Integer, CultureInfo.InvariantCulture, out long value)
            ? value
            : throw new InvalidDataException($"NBomber CSV value for '{name}' is not an integer.");

    private static double ParseDouble(string[] row, IReadOnlyDictionary<string, int> columns, string name)
        => double.TryParse(Get(row, columns, name), NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
            && double.IsFinite(value)
                ? value
                : throw new InvalidDataException($"NBomber CSV value for '{name}' is not a finite number.");

    private static List<string[]> ParseCsv(string content)
    {
        List<string[]> rows = [];
        List<string> fields = [];
        System.Text.StringBuilder field = new();
        bool quoted = false;

        for (int index = 0; index < content.Length; index++)
        {
            char current = content[index];
            if (quoted)
            {
                if (current == '"' && index + 1 < content.Length && content[index + 1] == '"')
                {
                    field.Append('"');
                    index++;
                }
                else if (current == '"')
                    quoted = false;
                else
                    field.Append(current);
            }
            else if (current == '"' && field.Length == 0)
                quoted = true;
            else if (current == ',')
            {
                fields.Add(field.ToString());
                field.Clear();
            }
            else if (current is '\r' or '\n')
            {
                if (current == '\r' && index + 1 < content.Length && content[index + 1] == '\n')
                    index++;
                if (field.Length > 0 || fields.Count > 0)
                {
                    fields.Add(field.ToString());
                    field.Clear();
                    rows.Add([.. fields]);
                    fields.Clear();
                }
            }
            else
                field.Append(current);
        }

        if (quoted)
            throw new InvalidDataException("NBomber CSV contains an unterminated quoted field.");
        if (field.Length > 0 || fields.Count > 0)
        {
            fields.Add(field.ToString());
            rows.Add([.. fields]);
        }

        return rows;
    }

    private sealed record ScenarioResult(
        string Name,
        long Requests,
        long Ok,
        long Failed,
        double OkRps,
        double Min,
        double Mean,
        double P50,
        double P95,
        double P99,
        double Max,
        double StdDev,
        double FailedRps,
        double FailedMean,
        double FailedP95,
        double DataTransferredMb,
        double TransferBytesPerSecond,
        string Duration);
}
