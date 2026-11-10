#!/usr/bin/env bash
# k6 Load Test Bootstrap Script (Linux/Mac)
# Starts the Kestrel host + WireMock, runs k6, then cleans up.

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/../../.." && pwd)"
HOST_PID=""

cleanup() {
    if [[ -n "$HOST_PID" ]] && kill -0 "$HOST_PID" 2>/dev/null; then
        echo "Stopping Kestrel host..."
        kill "$HOST_PID" 2>/dev/null || true
        wait "$HOST_PID" 2>/dev/null || true
    fi
}
trap cleanup EXIT INT TERM

echo "Starting k6 load test bootstrap..."

# Check for k6
if ! command -v k6 &> /dev/null; then
    echo "ERROR: k6 is not installed. Download from https://k6.io/docs/getting-started/installation/" >&2
    exit 1
fi

# Build the load test host
echo "Building load test project..."
dotnet build "$PROJECT_ROOT/Test/Orders.Integrations.Hub.LoadTests/Orders.Integrations.Hub.LoadTests.csproj" --no-restore

# Start the load test host in background
echo "Starting Kestrel host + WireMock container..."
dotnet run --project "$PROJECT_ROOT/Test/Orders.Integrations.Hub.LoadTests/Orders.Integrations.Hub.LoadTests.csproj" --no-build --host-only &
HOST_PID=$!

# Wait for host to be ready (check TCP port, not HTTP endpoint)
echo "Waiting for host to be ready on port 5099..."
READY=false
for i in $(seq 1 30); do
    if ! kill -0 "$HOST_PID" 2>/dev/null; then
        echo "ERROR: Load test host exited before becoming ready." >&2
        exit 1
    fi
    if (echo > /dev/tcp/127.0.0.1/5099) 2>/dev/null; then
        READY=true
        break
    fi
    sleep 2
done

# Check if host is ready
if [[ "$READY" != true ]]; then
    echo "ERROR: Host did not start within 60 seconds!" >&2
    exit 1
fi

echo "Host is ready! Running k6 load test..."

# Run k6
K6_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$K6_ROOT"

# Create reports directory if it doesn't exist
mkdir -p "$PROJECT_ROOT/reports"
export K6_REPORT_DIR="$PROJECT_ROOT/reports"
k6 run loadtest.js
echo "Check $PROJECT_ROOT/reports/ for per-run summary, JSON, and HTML reports."
