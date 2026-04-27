using System.Reflection;

using NetArchTest.Rules;

using Orders.Integrations.Hub.Core.Application.Ports.In.Http;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Middleware;

using TestResult = NetArchTest.Rules.TestResult;

namespace Orders.Integrations.Hub.ArchTests;

public class IntegrationsArchitectureTests
{
    private static readonly Assembly AppAssembly = typeof(Program).Assembly;

    private const string IntegrationsNamespace = "Orders.Integrations.Hub.Integrations";
    private const string CommonNamespace = "Orders.Integrations.Hub.Integrations.Common";
    private const string CoreAdaptersNamespace = "Orders.Integrations.Hub.Core.Adapters";
    private const string CoreInfrastructureNamespace = "Orders.Integrations.Hub.Core.Infrastructure";

    /// <summary>
    /// Discovers all integration namespaces dynamically.
    /// Any new integration folder is automatically picked up
    /// Rule: direct children of Integrations namespace, excluding Common.
    /// </summary>
    /// <returns>List of the Integrations namespaces discovered</returns>
    private static List<string> DiscoverIntegrationNamespaces()
        => AppAssembly.GetTypes()
            .Select(type => type.Namespace ?? string.Empty)
            .Where(name => 
                name.StartsWith(IntegrationsNamespace) && 
                !name.StartsWith(CommonNamespace) &&
                !name.Equals(IntegrationsNamespace)
            )
            .Select(name => {
                string remainder = name[(IntegrationsNamespace.Length + 1)..];
                string segment = remainder.Contains('.')
                    ? remainder[..remainder.IndexOf('.')]
                    : remainder;
                return $"{IntegrationsNamespace}.{segment}";
            })
            .Distinct()
            .OrderBy(name => name)
            .ToList();

    public static TheoryData<string> IntegrationNamespaces
        => new(DiscoverIntegrationNamespaces().ToArray());

    private static string Describe(string integration, string rule, TestResult result) {
        string name = integration[(IntegrationsNamespace.Length + 1)..];
        return $"[{name}] {rule}. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}";
    }
    
    private static string Domain(string ns) => $"{ns}.Domain";
    private static string Application(string ns) => $"{ns}.Application";
    private static string Infrastructure(string ns) => $"{ns}.Infrastructure";
    private static string Adapters(string ns) => $"{ns}.Adapters";
    private static string Ports(string ns) => $"{Application(ns)}.Ports";
    private static string PortsIn(string ns) => $"{Ports(ns)}.In";
    private static string PortsOut(string ns) => $"{Ports(ns)}.Out";
    private static string DomainContracts(string ns) => $"{Domain(ns)}.Contracts";

    #region 1. Integration isolation — modules must not know about each other

    [Fact]
    public void Discovery_ShouldFindAtLeastOneIntegration()
    {
        IReadOnlyList<string> integrations = DiscoverIntegrationNamespaces();
        Assert.NotEmpty(integrations);
    }

    [Fact]
    public void Discovery_ShouldNotIncludeCommon()
    {
        IReadOnlyList<string> integrations = DiscoverIntegrationNamespaces();
        Assert.DoesNotContain(integrations,
            ns => ns.Contains(".Common"));
    }

    [Fact]
    public void Integrations_ShouldNotDependOnEachOther()
    {
        var integrations = DiscoverIntegrationNamespaces();

        List<string> violations = [];

        foreach (string source in integrations)
        {
            string[] otherIntegrations = integrations
                .Where(ns => ns != source)
                .ToArray();

            TestResult? result = Types.InAssembly(AppAssembly)
                .That()
                .ResideInNamespace(source)
                .ShouldNot()
                .HaveDependencyOnAny(otherIntegrations)
                .GetResult();

            if (!result.IsSuccessful)
                violations.AddRange(result.FailingTypeNames ?? []);
        }

        Assert.True(violations.Count == 0,
            $"Integrations must not depend on each other. Violations:\n{string.Join("\n", violations)}");
    }

    #endregion

    #region 2. Integration domain independence

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void IntegrationDomain_ShouldNotDependOnApplication(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Domain(integration))
            .ShouldNot()
            .HaveDependencyOn(Application(integration))
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            Describe(integration, "Domain should not depend on its Application Layer", result)
        );
    }

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void IntegrationDomain_ShouldNotDependOnInfrastructure(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Domain(integration))
            .ShouldNot()
            .HaveDependencyOn(Infrastructure(integration))
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration, "Domain should not depend on its Infrastructure", result)
        );
    }

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void Domain_ShouldNotDependOnAdapters(string integration)
    {
        var result = Types.InAssembly(AppAssembly)
            .That().ResideInNamespace(Domain(integration))
            .ShouldNot().HaveDependencyOn(Adapters(integration))
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration,
                "Domain must not depend on Adapters",
                result));
    }

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void IntegrationDomain_ShouldNotDependOnCoreImplementation(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Domain(integration))
            .ShouldNot()
            .HaveDependencyOnAny([CoreAdaptersNamespace, CoreInfrastructureNamespace])
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration, "Domain should not depend on Core implementation", result)
            );
    }

    #endregion

    #region 3. Integration application layer rules

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void IntegrationApplication_ShouldNotDependOnInfrastructure(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Application(integration))
            .ShouldNot()
            .HaveDependencyOn(Infrastructure(integration))
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            Describe(integration,
                "Application must not depend on Infrastructure layer.",
                result)
        );
    }

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void IntegrationApplication_ShouldNotDependOnAdapters(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Application(integration))
            .ShouldNot()
            .HaveDependencyOn(Adapters(integration))
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration, "Application should not depend on Adapters", result)
        );
    }

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void Application_ShouldNotDependOnCoreImplementationDetails(string integration)
    {
        var result = Types.InAssembly(AppAssembly)
            .That().ResideInNamespace(Application(integration))
            .ShouldNot().HaveDependencyOnAny([CoreAdaptersNamespace, CoreInfrastructureNamespace])
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration,
                "Application must not depend on Core implementation details",
                result));
    }

    #endregion

    #region 5. Ports naming and placement

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void PortsIn_ShouldOnlyContainUseCaseImplementations(string integration)
    {
        // Skip if this integration has no Ports/In types yet
        bool hasPortsIn = AppAssembly.GetTypes()
            .Any(type => (type.Namespace ?? string.Empty).StartsWith(PortsIn(integration)));

        if (!hasPortsIn) return;

        var result = Types.InAssembly(AppAssembly)
            .That().ResideInNamespace(PortsIn(integration))
            .And().AreClasses()
            .Should().HaveNameEndingWith("UseCase")
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration,
                "Ports/In classes must end with UseCase",
                result));
    }

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void PortsOut_ShouldOnlyContainUseCaseImplementations(string integration)
    {
        bool hasPortsOut = AppAssembly.GetTypes()
            .Any(t => (t.Namespace ?? string.Empty).StartsWith(PortsOut(integration)));

        if (!hasPortsOut) return;

        var result = Types.InAssembly(AppAssembly)
            .That().ResideInNamespace(PortsOut(integration))
            .And().AreClasses()
            .Should().HaveNameEndingWith("UseCase")
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration,
                "Ports/Out classes must end with UseCase",
                result));
    }

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void DomainContracts_ShouldOnlyContainInterfaces(string integration)
    {
        bool hasContracts = AppAssembly.GetTypes()
            .Any(type => (type.Namespace ?? string.Empty).StartsWith(DomainContracts(integration)));

        if (!hasContracts) return;

        var result = Types.InAssembly(AppAssembly)
            .That().ResideInNamespace(DomainContracts(integration))
            .Should().BeInterfaces()
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration,
                "Domain/Contracts must only contain interfaces",
                result));
    }

    #endregion
    
    #region 6. Adapters rules

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void Adapters_ShouldImplementIEndpoint(string integration)
    {
        bool hasAdapters = AppAssembly.GetTypes()
            .Any(type => (type.Namespace ?? string.Empty).StartsWith(Adapters(integration)));

        if (!hasAdapters) return;

        var result = Types.InAssembly(AppAssembly)
            .That().ResideInNamespace(Adapters(integration))
            .And().AreClasses()
            .Should().ImplementInterface(typeof(IEndpoint))
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration, "All classes in Adapters must implement IEndpoint", result));
    }

    #endregion

    #region 7. Infrastructure rules
    
    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void Infrastructure_ShouldNotDependOnAdapters(string integration)
    {
        var result = Types.InAssembly(AppAssembly)
            .That().ResideInNamespace(Infrastructure(integration))
            .ShouldNot().HaveDependencyOn(Adapters(integration))
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration, "Infrastructure must not depend on Adapters", result));
    }

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void SignatureStrategies_ShouldResideInInfrastructure(string integration)
    {
        var result = Types.InAssembly(AppAssembly)
            .That()
            .ResideInNamespace(integration)
            .And().AreClasses()
            .And().ImplementInterface(typeof(IWebhookSignatureValidator))
            .Should().ResideInNamespace(Infrastructure(integration))
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration, "IWebhookSignatureValidator implementations must be in Infrastructure", result));
    }

    #endregion

    #region 4. Common module rules — shared contracts, not shared implementations

    [Fact]
    public void Common_ShouldNotDependOnAnyIntegration()
    {
        string[] allIntegrations = DiscoverIntegrationNamespaces().ToArray();

        var result = Types.InAssembly(AppAssembly)
            .That().ResideInNamespace(CommonNamespace)
            .ShouldNot().HaveDependencyOnAny(allIntegrations)
            .GetResult();

        Assert.True(result.IsSuccessful,
            "Common must not depend on any integration. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Common_ShouldNotDependOnCoreAdaptersOrInfrastructure()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(CommonNamespace)
            .ShouldNot()
            .HaveDependencyOnAny([CoreAdaptersNamespace, CoreInfrastructureNamespace])
            .GetResult();

        Assert.True(result.IsSuccessful,
            "Common should not depend on Core implementation details. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void CommonContracts_ShouldBeInterfaces()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{CommonNamespace}.Contracts")
            .Should()
            .BeInterfaces()
            .GetResult();

        Assert.True(result.IsSuccessful,
            "Common contracts should all be interfaces. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 5. Signature validation contracts

    [Fact]
    public void SignatureValidators_ShouldImplementISignatureValidator()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That().HaveNameEndingWith("SignatureValidator")
            .And().AreClasses()
            .And().DoNotHaveName(nameof(WebhookSignatureFilter<,,>))
            .And().DoNotHaveName(nameof(IWebhookSignatureValidator))
            .And().DoNotImplementInterface(typeof(IWebhookSignatureValidator))
            .Should().ImplementInterface(typeof(ISignatureValidator))
            .GetResult();

        Assert.True(result.IsSuccessful,
            "All signature validators should implement ISignatureValidator. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void SignatureStrategies_ShouldImplementIWebhookSignatureValidator()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveNameEndingWith("SignatureStrategy")
            .And()
            .AreClasses()
            .Should()
            .ImplementInterface(typeof(IWebhookSignatureValidator))
            .GetResult();

        Assert.True(result.IsSuccessful,
            "All signature strategies should implement IWebhookSignatureValidator. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void SignatureValidators_ShouldResideInCommonOrInfrastructure()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ImplementInterface(typeof(ISignatureValidator))
            .And()
            .AreClasses()
            .Should()
            .ResideInNamespaceStartingWith($"{CommonNamespace}.Validators")
            .GetResult();

        Assert.True(result.IsSuccessful,
            "ISignatureValidator implementations should live in Common.Validators. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void SignatureStrategies_ShouldResideInIntegrationInfrastructure()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ImplementInterface(typeof(IWebhookSignatureValidator))
            .And()
            .AreClasses()
            .Should()
            .ResideInNamespaceContaining(".Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful,
            "IWebhookSignatureValidator implementations should live in integration Infrastructure folders. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 6. Webhook endpoints — must use the signature filter

    [Fact]
    public void WebhookEndpoints_ShouldResideInAdapters()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveNameEndingWith("WebhookEndpoint")
            .And()
            .AreClasses()
            .Should()
            .ResideInNamespaceContaining(".Adapters")
            .GetResult();

        Assert.True(result.IsSuccessful,
            "Webhook endpoints should reside in Adapters. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void WebhookEndpoints_ShouldImplementIEndpoint()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveNameEndingWith("WebhookEndpoint")
            .And()
            .AreClasses()
            .Should()
            .ImplementInterface(typeof(IEndpoint))
            .GetResult();

        Assert.True(result.IsSuccessful,
            "Webhook endpoints should implement IEndpoint. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 7. Naming conventions

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void IntegrationUseCases_ShouldBeInPortsNamespace(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Application(integration))
            .And()
            .HaveNameEndingWith("UseCase")
            .And()
            .AreClasses()
            .Should()
            .ResideInNamespaceContaining(".Ports.")
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration, "UseCase implementations should reside in Ports", result)
        );
    }

    [Theory]
    [MemberData(nameof(IntegrationNamespaces))]
    public void IntegrationKey_ShouldBeSealed(string integration)
    {
        var result = Types.InAssembly(AppAssembly)
            .That().ResideInNamespace(integration)
            .And().HaveNameEndingWith("IntegrationKey")
            .Should().BeSealed()
            .GetResult();

        Assert.True(result.IsSuccessful,
            Describe(integration, "IntegrationKey classes must be sealed", result));
    }

    [Fact]
    public void DependencyInjection_ShouldBeStaticAndSealed()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(IntegrationsNamespace)
            .And()
            .HaveNameEndingWith("DependencyInjection")
            .Should()
            .BeStatic()
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Integration DependencyInjection classes should be static. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion
}