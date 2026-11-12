# k6 Load Test Bootstrap Script (Windows)
# Starts the Kestrel host + WireMock, runs k6, then cleans up.

$ErrorActionPreference = 'Stop'
$PSScriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent (Split-Path -Parent (Split-Path -Parent $PSScriptRoot))

Write-Host "Starting k6 load test bootstrap..." -ForegroundColor Cyan

# Check for k6
if (-not (Get-Command k6 -ErrorAction SilentlyContinue)) {
    Write-Host "ERROR: k6 is not installed. Download from https://k6.io/docs/getting-started/installation/" -ForegroundColor Red
    exit 1
}

# Build the load test host
Write-Host "Building load test project..." -ForegroundColor Yellow
dotnet build "$ProjectRoot\Test\Orders.Integrations.Hub.LoadTests\Orders.Integrations.Hub.LoadTests.csproj" --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# Start the load test host in background
Write-Host "Starting Kestrel host + WireMock container..." -ForegroundColor Yellow
$hostProcess = Start-Process -FilePath "dotnet" -ArgumentList `
    "run --project `"$ProjectRoot\Test\Orders.Integrations.Hub.LoadTests\Orders.Integrations.Hub.LoadTests.csproj`" --no-build --host-only" `
    -NoNewWindow -PassThru

# Wait for host to be ready (check TCP port, not HTTP endpoint)
Write-Host "Waiting for host to be ready on port 5099..." -ForegroundColor Yellow
$K6Root = Split-Path -Parent $PSScriptRoot
$ReportsDir = Join-Path $ProjectRoot "reports"
New-Item -ItemType Directory -Path $ReportsDir -Force | Out-Null
$oldLocation = Get-Location
$k6ExitCode = 1

try {
    $ready = $false
    for ($i = 0; $i -lt 30; $i++) {
        if ($hostProcess.HasExited) { throw "Load test host exited early (code $($hostProcess.ExitCode))." }
        try {
            $tcpClient = New-Object System.Net.Sockets.TcpClient
            $tcpClient.ConnectAsync("127.0.0.1", 5099).Wait(2000)
            if ($tcpClient.Connected) { $ready = $true; $tcpClient.Close(); break }
            $tcpClient.Close()
        } catch { }
        Start-Sleep -Seconds 2
    }
    if (-not $ready) { throw "Host did not start within 60 seconds." }

    Write-Host "Host is ready! Running k6 load test..." -ForegroundColor Green
    Set-Location $K6Root
    $env:K6_REPORT_DIR = $ReportsDir.Replace('\', '/')
    k6 run loadtest.js
    $k6ExitCode = $LASTEXITCODE
}
catch {
    Write-Host "ERROR: $_" -ForegroundColor Red
    $k6ExitCode = 1
}
finally {
    Remove-Item Env:\K6_REPORT_DIR -ErrorAction SilentlyContinue
    Set-Location $oldLocation
    if ($hostProcess -and -not $hostProcess.HasExited) {
        Write-Host "Stopping Kestrel host..." -ForegroundColor Yellow
        Stop-Process -Id $hostProcess.Id -Force -ErrorAction SilentlyContinue
        $hostProcess.WaitForExit()
    }
}

if ($k6ExitCode -ne 0) { exit $k6ExitCode }
Write-Host "k6 load test completed successfully! Check $ReportsDir for per-run reports." -ForegroundColor Green
