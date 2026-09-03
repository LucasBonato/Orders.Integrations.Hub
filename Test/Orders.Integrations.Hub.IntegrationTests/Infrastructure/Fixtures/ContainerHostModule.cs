using System.Runtime.CompilerServices;

using DotNet.Testcontainers.Configurations;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;

internal static class ContainerHostModule {
    [ModuleInitializer]
    internal static void Initialize() {
        string? env = Environment.GetEnvironmentVariable("TESTCONTAINERS_HOST_OVERRIDE");
        if (!string.IsNullOrWhiteSpace(env))
            TestcontainersSettings.DockerHostOverride = env.Trim();
    }
}