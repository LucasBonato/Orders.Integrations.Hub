using System.Reflection;

using NetArchTest.Rules;

using TestResult = NetArchTest.Rules.TestResult;

namespace Orders.Integrations.Hub.ArchTests;

public class TestConventionsTests
{
    private static readonly Assembly TestAssembly = typeof(TestConventionsTests).Assembly;

    /// <summary>
    /// Ensures that the dead FluentAssertions dependency is not being used.
    /// FluentAssertions v7 is declared in Directory.Packages.props but all tests
    /// use vanilla xUnit Assert.*. Remove the package when convenient.
    /// </summary>
    [Fact]
    public void FluentAssertions_ShouldNotBeUsedInTests()
    {
        TestResult? result = Types.InAssembly(TestAssembly)
            .That()
            .AreClasses()
            .ShouldNot()
            .HaveDependencyOn("FluentAssertions")
            .GetResult();

        Assert.True(result.IsSuccessful,
            "FluentAssertions package is declared but should not be referenced. " +
            "Use xUnit Assert.* instead, or remove the package reference. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    /// <summary>
    /// Verifies that all integration namespaces follow the naming convention.
    /// Each integration namespace root should be a direct child of Integrations.
    /// </summary>
    [Fact]
    public void IntegrationNamespaces_ShouldHaveConsistentNaming()
    {
        List<string> integrations = DiscoverIntegrationNamespaces();

        Assert.All(integrations, ns => {
            string name = ns.Split('.').Last();
            Assert.Matches("^[A-Z][a-zA-Z0-9]+$", name);
        });
    }

    private static List<string> DiscoverIntegrationNamespaces()
    {
        Assembly appAssembly = typeof(Program).Assembly;
        const string integrationsNamespace = "Orders.Integrations.Hub.Integrations";
        const string commonNamespace = "Orders.Integrations.Hub.Integrations.Common";

        return appAssembly.GetTypes()
            .Select(type => type.Namespace ?? string.Empty)
            .Where(name =>
                name.StartsWith(integrationsNamespace) &&
                !name.StartsWith(commonNamespace) &&
                !name.Equals(integrationsNamespace)
            )
            .Select(name => {
                string remainder = name[(integrationsNamespace.Length + 1)..];
                string segment = remainder.Contains('.')
                    ? remainder[..remainder.IndexOf('.')]
                    : remainder;
                return $"{integrationsNamespace}.{segment}";
            })
            .Distinct()
            .OrderBy(name => name)
            .ToList();
    }
}