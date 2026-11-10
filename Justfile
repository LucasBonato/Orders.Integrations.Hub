export PROJECT := "Src/Orders.Integrations.Hub"
export SOLUTION := "Orders.Integrations.Hub.slnx"

set dotenv-load

# List all recipes
[private]
default:
    @just --list

# =========================
# Development
# =========================

# Run Project
[group("dev")]
run:
    dotnet run --project {{PROJECT}}

# Run project with watch
[group("dev")]
watch:
    dotnet watch run --project {{PROJECT}}

# Restore the project
[group("dev")]
restore:
    dotnet restore {{SOLUTION}}

# Build the project
[group("dev")]
build:
    dotnet build {{SOLUTION}}

# Clean Solution
[group("dev")]
clean:
    dotnet clean {{SOLUTION}}

# =========================
# Tests
# =========================

# Run all tests
[group("test")]
test:
    dotnet test {{SOLUTION}}

# Run all tests with watch
[group("test")]
test-watch:
    dotnet watch test {{SOLUTION}}

# Run all tests with coverage
[group("test")]
coverage:
    dotnet test {{SOLUTION}} \
        --coverlet \
        --coverlet-output-format cobertura

# =========================
# Formatting
# =========================

# Format code
[group("format")]
format:
    dotnet format {{SOLUTION}}

# Show warnings of the lint
[group("format")]
lint:
    dotnet build {{SOLUTION}} -warnaserror

# Run all checks for format and tests
[group("format")]
check: format lint test
    @echo "All checks passed"

# =========================
# Containers
# =========================

# Up all containers from compose
[group("containers")]
up:
    docker compose up -d

# Dowm all containers from compose
[group("containers")]
down:
    docker compose down

# See logs of containers
[group("containers")]
logs:
    docker compose logs -f

# Rebuild containers
[group("containers")]
rebuild:
    docker compose up -d --build

# Process Status of containers
[group("containers")]
ps:
    docker compose ps

# =========================
# OpenTelemetry
# =========================

# Show url for dashboard OpenTelemetry is exporting
[group("otel")]
dashboard:
    @echo "Aspire dashboard: $OTEL_EXPORTER_OTLP_ENDPOINT"

# =========================
# Utilities
# =========================

# List all packages of the solution
[group("util")]
deps:
    dotnet list {{SOLUTION}} package

# Show outdated packages
[group("util")]
outdated:
    dotnet list {{SOLUTION}} package --outdated

# =========================
# Load Tests
# =========================

export LOADTESTS_NBOMBER := "Test/Orders.Integrations.Hub.LoadTests"
export LOADTESTS_K6 := "Test/Orders.Integrations.Hub.LoadTests.K6"

# NBomber load test (default profiles, ~30s duration)
# Usage: just loadtest-nbomber [vus] [duration]
[group("loadtest")]
loadtest-nbomber vus="3" duration="00:00:30":
    @echo "Running NBomber load test ({{vus}} vus, {{duration}} duration)..."
    env LOADTEST_WEBHOOK_COPIES={{vus}} LOADTEST_WEBHOOK_DURATION={{duration}} LOADTEST_CORE_COPIES={{vus}} LOADTEST_CORE_DURATION={{duration}} LOADTEST_RAPPI_PING_COPIES=2 LOADTEST_RAPPI_PING_DURATION={{duration}} dotnet run --project {{LOADTESTS_NBOMBER}}/Orders.Integrations.Hub.LoadTests.csproj

# NBomber stress load test (higher load, ~2min duration)
# Usage: just loadtest-nbomber-stress [vus] [duration]
[group("loadtest")]
loadtest-nbomber-stress vus="10" duration="00:02:00":
    @echo "Running NBomber stress load test ({{vus}} vus, {{duration}} duration)..."
    env LOADTEST_STRESS_WEBHOOK_COPIES={{vus}} LOADTEST_STRESS_WEBHOOK_DURATION={{duration}} LOADTEST_STRESS_CORE_COPIES={{vus}} LOADTEST_STRESS_CORE_DURATION={{duration}} dotnet run --project {{LOADTESTS_NBOMBER}}/Orders.Integrations.Hub.LoadTests.csproj -- --stress

# k6 load test (default profiles)
# Usage: just loadtest-k6 [vus] [duration]
[group("loadtest")]
loadtest-k6 vus="3" duration="30s":
    @echo "Running k6 load test ({{vus}} vus, {{duration}} duration)..."
    @WEBHOOK_VUS={{vus}} WEBHOOK_DURATION={{duration}} CORE_VUS={{vus}} CORE_DURATION={{duration}} powershell -ExecutionPolicy Bypass -File "$(cygpath -w '{{LOADTESTS_K6}}/scripts/run.ps1')"

# k6 stress load test (higher load)
# Usage: just loadtest-k6-stress [vus] [duration]
[group("loadtest")]
loadtest-k6-stress vus="10" duration="2m":
    @echo "Running k6 stress load test ({{vus}} vus, {{duration}} duration)..."
    @STRESS=true STRESS_WEBHOOK_VUS={{vus}} STRESS_CORE_VUS={{vus}} STRESS_DURATION={{duration}} powershell -ExecutionPolicy Bypass -File "$(cygpath -w '{{LOADTESTS_K6}}/scripts/run.ps1')"
