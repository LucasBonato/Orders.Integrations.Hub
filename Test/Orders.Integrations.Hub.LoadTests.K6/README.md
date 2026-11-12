# k6 Load Tests for Orders.Integrations.Hub

k6 exercises signed partner webhooks and Core HTTP endpoints concurrently. All partner outbound calls are routed to WireMock by the shared C# load-test host; no real partner API should be contacted.

## Prerequisites and quick start

- k6 installed and available on `PATH`
- .NET 10 SDK
- Docker or Podman available to Testcontainers for WireMock

Windows PowerShell:

```powershell
.\scripts\run.ps1
```

Linux/macOS:

```bash
chmod +x scripts/run.sh
./scripts/run.sh
```

The bootstrap builds and starts the shared Kestrel/WireMock host, waits for port 5099, runs k6, writes reports under the repository `reports/` directory, and stops the host even if k6 fails.

## Manual run

Start the app host in one terminal:

```powershell
dotnet run --project Test/Orders.Integrations.Hub.LoadTests/Orders.Integrations.Hub.LoadTests.csproj -- --host-only
```

Then, from this directory:

```shell
k6 run loadtest.js
```

## Profiles and environment variables

Pass environment variables using `k6 run --env NAME=value loadtest.js`, or set them in the shell before using the bootstrap.

| Variable | Default | Description |
|---|---:|---|
| `BASE_URL` | `http://127.0.0.1:5099` | Target app URL |
| `WEBHOOK_VUS` | `3` | VUs per webhook scenario |
| `WEBHOOK_DURATION` | `30s` | Steady webhook load duration |
| `CORE_VUS` | `3` | VUs per Core scenario |
| `CORE_DURATION` | `30s` | Steady Core load duration |
| `RAPPI_PING_VUS` | `2` | VUs for Rappi ping |
| `RAMP_UP_DURATION` | `5s` | Ramp to target VUs |
| `RAMP_DOWN_DURATION` | `5s` | Ramp down after steady load |
| `STRESS` | `false` | Use stress profile |
| `STRESS_WEBHOOK_VUS` | `10` | Stress webhook VUs per scenario |
| `STRESS_CORE_VUS` | `10` | Stress Core VUs per scenario |
| `STRESS_DURATION` | `2m` | Stress steady-load duration |
| `MAX_ERROR_RATE` | `5` | Maximum failed-request percentage; affects k6 exit status |
| `MAX_P95_MS` | `5000` | Maximum p95 latency in milliseconds; affects k6 exit status |
| `K6_REPORT_DIR` | `.` | Report directory (bootstrap sets repository `reports/`) |

Example:

```shell
k6 run --env WEBHOOK_VUS=5 --env CORE_VUS=2 --env WEBHOOK_DURATION=1m loadtest.js
k6 run --env STRESS=true --env STRESS_WEBHOOK_VUS=10 loadtest.js
```

The ramp-up/down stages are additional to the configured steady duration. Invalid VU counts and threshold values fail early rather than silently falling back to defaults.

## Scenarios

Webhook scenarios cover iFood, Food99, and Rappi create/cancel/other/ping endpoints. Core scenarios cover order status, disputes, and cancellation reasons for each integration; provider-specific status values and cancellation reasons are used. Each scenario has independent error-rate and p95 thresholds, as well as global thresholds.

## Reports

Each run generates unique timestamped files in `reports/`:

- `k6-{timestamp}-summary.txt`: per-scenario requests, errors, latency percentiles, and verdict
- `k6-{timestamp}-summary.json`: machine-readable results and threshold settings
- `k6-{timestamp}-report.html`: standalone HTML summary
- Sub-0.001 ms percentile values are displayed as `<0.001` rather than `0.00` to avoid implying exact zero latency.

Threshold violations produce a non-zero k6 exit code, which the bootstrap propagates after cleaning up the host.
