using Microsoft.Extensions.Configuration;

namespace Orders.Integrations.Hub.TestCommon.Configuration;

/// <summary>Single source of truth for static, committed test configuration values.</summary>
public static class TestConfiguration {
    private static readonly Lazy<IConfigurationRoot> _root = new(
        () => new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: false)
            .Build()
    );

    public static string GetRequired(string key) 
        => _root.Value[key] 
            ?? throw new InvalidOperationException($"Missing required test configuration value: '{key}'");
}