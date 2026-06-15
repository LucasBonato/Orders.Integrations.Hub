export PROJECT := "Src/Orders.Integrations.Hub"

set dotenv-load

# List all recipes
[private]
default:
    @just --list
    
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
    dotnet restore

# Build the project
[group("dev")]
build:
    dotnet build

# Clean Solution
[group("dev")]
clean:
    dotnet clean
    
# Run all tests
[group("test")]
test:
    dotnet test Test/*

# Run all tests with watch
[group("test")]
test-watch:
    dotnet watch test Test/*

# Run all tests with coverage
[group("test")]
coverage:
    dotnet test Test/* --collect:"XPlat Code Coverage"
    
# Format code
[group("format")]
format:
    dotnet format

# Show warnings of the lint
[group("format")]
lint:
    dotnet build -warnaserror

# Run all checks for format and tests
[group("format")]
check: format lint test
    @echo "All checks passed"

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

# Show url for dashboard OpenTelemetry is exporting
[group("otel")]
dashboard:
    @echo "Aspire dashboard: $OTEL_EXPORTER_OTLP_ENDPOINT"


# List all packages of the solution
[group("util")]
deps:
    dotnet list package

# Show outdated packages
[group("util")]
outdated:
    dotnet list package --outdated