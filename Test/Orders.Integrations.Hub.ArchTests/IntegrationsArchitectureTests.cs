using NetArchTest.Rules;

using Orders.Integrations.Hub.Core.Application.Ports.In.Http;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Middleware;

namespace Orders.Integrations.Hub.ArchTests;

public class IntegrationsArchitectureTests
{
    private const string IntegrationsNamespace = "Orders.Integrations.Hub.Integrations";
    private const string CommonNamespace = "Orders.Integrations.Hub.Integrations.Common";
    private const string CoreAdaptersNamespace = "Orders.Integrations.Hub.Core.Adapters";
    private const string CoreInfrastructureNamespace = "Orders.Integrations.Hub.Core.Infrastructure";

    private static readonly string[] AllIntegrations = ["IFood", "Food99", "Rappi"];

    private static string Integration(string name) => $"{IntegrationsNamespace}.{name}";
    private static string Domain(string integration) => $"{Integration(integration)}.Domain";
    private static string Application(string integration) => $"{Integration(integration)}.Application";
    private static string Infrastructure(string integration) => $"{Integration(integration)}.Infrastructure";
    private static string Adapters(string integration) => $"{Integration(integration)}.Adapters";

    #region 1. Integration isolation — modules must not know about each other

    [Theory]
    [InlineData("IFood", "Rappi")]
    [InlineData("IFood", "Food99")]
    [InlineData("Rappi", "IFood")]
    [InlineData("Rappi", "Food99")]
    [InlineData("Food99", "IFood")]
    [InlineData("Food99", "Rappi")]
    public void Integration_ShouldNotDependOnAnotherIntegration(string source, string target)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Integration(source))
            .ShouldNot()
            .HaveDependencyOn(Integration(target))
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{source} should not depend on {target}. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }
    
    #endregion

    #region 2. Integration domain independence

    [Theory]
    [InlineData("IFood")]
    [InlineData("Food99")]
    [InlineData("Rappi")]
    public void IntegrationDomain_ShouldNotDependOnApplication(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Domain(integration))
            .ShouldNot()
            .HaveDependencyOn(Application(integration))
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{integration} Domain should not depend on its Application layer. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [InlineData("IFood")]
    [InlineData("Food99")]
    [InlineData("Rappi")]
    public void IntegrationDomain_ShouldNotDependOnInfrastructure(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Domain(integration))
            .ShouldNot()
            .HaveDependencyOn(Infrastructure(integration))
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{integration} Domain should not depend on its Infrastructure layer. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [InlineData("IFood")]
    [InlineData("Food99")]
    [InlineData("Rappi")]
    public void IntegrationDomain_ShouldNotDependOnCoreImplementation(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Domain(integration))
            .ShouldNot()
            .HaveDependencyOnAny([CoreAdaptersNamespace, CoreInfrastructureNamespace])
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{integration} Domain should not depend on Core implementation details. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 3. Integration application layer rules

    [Theory]
    [InlineData("IFood")]
    [InlineData("Food99")]
    [InlineData("Rappi")]
    public void IntegrationApplication_ShouldNotDependOnInfrastructure(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Application(integration))
            .ShouldNot()
            .HaveDependencyOn(Infrastructure(integration))
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{integration} Application should not depend on Infrastructure. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [InlineData("IFood")]
    [InlineData("Food99")]
    [InlineData("Rappi")]
    public void IntegrationApplication_ShouldNotDependOnAdapters(string integration)
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(Application(integration))
            .ShouldNot()
            .HaveDependencyOn(Adapters(integration))
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{integration} Application should not depend on Adapters. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 4. Common module rules — shared contracts, not shared implementations

    [Fact]
    public void Common_ShouldNotDependOnAnySpecificIntegration()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(CommonNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(AllIntegrations.Select(Integration).ToArray())
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Common should not depend on any specific integration. " +
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
            $"Common should not depend on Core implementation details. " +
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
            $"Common contracts should all be interfaces. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 5. Signature validation contracts

    [Fact]
    public void SignatureValidators_ShouldImplementISignatureValidator()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveNameEndingWith("SignatureValidator")
            .And()
            .AreClasses()
            .And()
            .DoNotHaveName(nameof(WebhookSignatureFilter<,,>))
            .And()
            .DoNotHaveName(nameof(IWebhookSignatureValidator))
            .And()
            .DoNotImplementInterface(typeof(IWebhookSignatureValidator))
            .Should()
            .ImplementInterface(typeof(ISignatureValidator))
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"All signature validators should implement ISignatureValidator. " +
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
            $"All signature strategies should implement IWebhookSignatureValidator. " +
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
            $"ISignatureValidator implementations should live in Common.Validators. " +
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
            $"IWebhookSignatureValidator implementations should live in integration Infrastructure folders. " +
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
            $"Webhook endpoints should reside in Adapters. " +
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
            $"Webhook endpoints should implement IEndpoint. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 7. Naming conventions

    [Theory]
    [InlineData("IFood")]
    [InlineData("Food99")]
    [InlineData("Rappi")]
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
            $"{integration} use case implementations should reside in Ports namespace. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void IntegrationKeys_ShouldBeSealed()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(IntegrationsNamespace)
            .And()
            .HaveNameEndingWith("IntegrationKey")
            .Should()
            .BeSealed()
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Integration key classes should be sealed. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
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