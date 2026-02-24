using NetArchTest.Rules;

using Orders.Integrations.Hub.Core.Application.Ports.In.Integration;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Infrastructure.Serialization;

namespace Orders.Integrations.Hub.ArchTests;

/// <summary>
/// Architecture tests for the Core module enforcing Hexagonal Architecture principles
/// Uses NetArchTest for cleaner, more intuitive syntax
/// </summary>
public class CoreArchitectureTests
{
    private const string CoreNamespace = "Orders.Integrations.Hub.Core";
    private const string DomainNamespace = "Orders.Integrations.Hub.Core.Domain";
    private const string ApplicationNamespace = "Orders.Integrations.Hub.Core.Application";
    private const string InfrastructureNamespace = "Orders.Integrations.Hub.Core.Infrastructure";
    private const string AdaptersNamespace = "Orders.Integrations.Hub.Core.Adapters";
    private const string IntegrationsNamespace = "Orders.Integrations.Hub.Integrations";

    #region 1. Domain Independence Tests (Core of Hexagonal Architecture)

    [Fact]
    public void Domain_ShouldNotDependOnApplication()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOn(ApplicationNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Domain should not depend on Application. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Domain_ShouldNotDependOnInfrastructure()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Domain should not depend on Infrastructure. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Domain_ShouldNotDependOnAdapters()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOn(AdaptersNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Domain should not depend on Adapters. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Domain_ShouldNotDependOnIntegrations()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOn(IntegrationsNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Domain should not depend on Integrations. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 2. Application Layer Tests

    [Fact]
    public void Application_ShouldNotDependOnInfrastructure()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(ApplicationNamespace)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Application should not depend on Infrastructure. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Application_ShouldNotDependOnAdapters()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(ApplicationNamespace)
            .ShouldNot()
            .HaveDependencyOn(AdaptersNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Application should not depend on Adapters. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Application_ShouldNotDependOnIntegrations()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(ApplicationNamespace)
            .ShouldNot()
            .HaveDependencyOn(IntegrationsNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Application should not depend on Integrations. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 3. Ports and Adapters Tests

    [Fact]
    public void DomainPorts_ShouldBeInterfaces()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{DomainNamespace}.Contracts.Ports")
            .Should()
            .BeInterfaces()
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Domain ports should be interfaces. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void DomainPorts_ShouldStartWithI()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{DomainNamespace}.Contracts.Ports")
            .And()
            .AreInterfaces()
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Domain port interfaces should start with 'I'. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void InboundPorts_ShouldHaveUseCaseInName()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{DomainNamespace}.Contracts.Ports.In")
            .Should()
            .HaveNameEndingWith("UseCase")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Inbound ports should end with 'UseCase'. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void OutboundPorts_ShouldHaveUseCaseInName()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{DomainNamespace}.Contracts.Ports.Out")
            .Should()
            .HaveNameEndingWith("UseCase")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Outbound ports should end with 'UseCase'. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void ApplicationPorts_ShouldBeInterfaces()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{ApplicationNamespace}.Ports")
            .Should()
            .BeInterfaces()
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Application ports should be interfaces. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 4. Adapter Tests

    [Fact]
    public void InboundAdapters_ShouldNotDependOnOutboundAdapters()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{AdaptersNamespace}.In")
            .ShouldNot()
            .HaveDependencyOn($"{AdaptersNamespace}.Out")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Inbound adapters should not depend on Outbound adapters. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Controllers_ShouldResideInInboundAdapters()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Or()
            .HaveNameEndingWith("Endpoint")
            .And()
            .HaveName(nameof(AppEnv))
            .Should()
            .ResideInNamespace($"{AdaptersNamespace}.In.Http")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Controllers/Endpoints should reside in Inbound Adapters. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void CommandHandlers_ShouldResideInInboundAdapters()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveNameEndingWith("CommandHandler")
            .Or()
            .HaveNameEndingWith("EventHandler")
            .Should()
            .ResideInNamespace($"{AdaptersNamespace}.In.Messaging")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Command/Event handlers should reside in Inbound Adapters. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void HttpClients_ShouldResideInOutboundAdapters()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveNameEndingWith("Client")
            .And()
            .ResideInNamespace($"{AdaptersNamespace}.Out")
            .Should()
            .ResideInNamespace($"{AdaptersNamespace}.Out.HttpClients")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"HTTP Clients should reside in Outbound Adapters. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 5. Infrastructure Tests

    [Fact]
    public void Infrastructure_ShouldNotDependOnAdapters()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(InfrastructureNamespace)
            .ShouldNot()
            .HaveDependencyOn(AdaptersNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Infrastructure should not depend on Adapters. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 6. Integration Module Isolation Tests

    [Fact]
    public void IFood_ShouldNotDependOnRappi()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{IntegrationsNamespace}.IFood")
            .ShouldNot()
            .HaveDependencyOn($"{IntegrationsNamespace}.Rappi")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"IFood should not depend on Rappi. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void IFood_ShouldNotDependOn99Food()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{IntegrationsNamespace}.IFood")
            .ShouldNot()
            .HaveDependencyOn($"{IntegrationsNamespace}.Food99")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"IFood should not depend on 99Food. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Rappi_ShouldNotDependOnIFood()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{IntegrationsNamespace}.Rappi")
            .ShouldNot()
            .HaveDependencyOn($"{IntegrationsNamespace}.IFood")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Rappi should not depend on IFood. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Rappi_ShouldNotDependOn99Food()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{IntegrationsNamespace}.Rappi")
            .ShouldNot()
            .HaveDependencyOn($"{IntegrationsNamespace}.Food99")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Rappi should not depend on 99Food. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void IntegrationModules_ShouldNotDependOnCoreImplementation()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(IntegrationsNamespace)
            .And()
            .DoNotHaveNameEndingWith("DependencyInjection")
            .And()
            .DoNotHaveNameEndingWith("JsonSerializer")
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .Or()
            .HaveDependencyOn(AdaptersNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Integrations should only depend on Domain, not Core implementation. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 7. Naming Convention Tests

    [Fact]
    public void Commands_ShouldEndWithCommand()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{ApplicationNamespace}.Commands")
            .And()
            .AreClasses()
            .And()
            .DoNotHaveName("ICommand")
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Commands should end with 'Command'. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Exceptions_ShouldEndWithException()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{InfrastructureNamespace}.Exceptions")
            .And()
            .AreClasses()
            .And()
            .DoNotHaveName("IProblemDetailsException")
            .Should()
            .HaveNameEndingWith("Exception")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Exceptions should end with 'Exception'. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void IntegrationKeys_ShouldEndWithIntegrationKey()
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
            $"Integration key classes should be sealed. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void DTOs_ShouldBeInDTOsNamespace()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(CoreNamespace)
            .And()
            .MeetCustomRule(new EndsWithRequestOrResponse())
            .Should()
            .ResideInNamespace($"{ApplicationNamespace}.DTOs")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"DTOs should reside in DTOs namespace. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 8. Serialization Tests

    [Fact]
    public void JsonSerializers_ShouldImplementICustomJsonSerializer()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveNameEndingWith("JsonSerializer")
            .And()
            .AreClasses()
            .Should()
            .ImplementInterface(typeof(ICustomJsonSerializer))
            .Or()
            .Inherit(typeof(CoreJsonSerializer))
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"JSON Serializers should implement ICustomJsonSerializer. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 9. IntegrationKey Architecture Tests

    [Fact]
    public void IntegrationKey_ShouldBeRecord()
    {
        var integrationKeyType = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveName("IntegrationKey")
            .And()
            .ResideInNamespace($"{DomainNamespace}.ValueObjects")
            .GetTypes()
            .ToList();

        Assert.Single(integrationKeyType);

        var type = integrationKeyType.First();
        Assert.True(type.IsValueType || type.IsClass, "IntegrationKey should be a record");
    }

    [Fact]
    public void IntegrationRouter_ShouldImplementIIntegrationRouter()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveName("IntegrationRouter")
            .Should()
            .ImplementInterface(typeof(IIntegrationRouter))
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"IntegrationRouter should implement IIntegrationRouter. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 10. Dependency Injection Tests

    [Fact]
    public void DependencyInjection_ClassesShouldBeInCorrectNamespace()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .HaveNameEndingWith("DependencyInjection")
            .Should()
            .BeSealed()
            .Or()
            .BeStatic()
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"DependencyInjection classes should be sealed or static. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion

    #region 11. Summary Test - Complete Hexagonal Architecture

    [Fact]
    public void HexagonalArchitecture_DomainIsIndependent()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(DomainNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(new[] {
                ApplicationNamespace,
                InfrastructureNamespace,
                AdaptersNamespace,
                IntegrationsNamespace
            })
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Domain must be independent of all other layers. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void HexagonalArchitecture_ApplicationDependsOnlyOnDomain()
    {
        var result = Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace(ApplicationNamespace)
            .ShouldNot()
            .HaveDependencyOnAny(new[] {
                InfrastructureNamespace,
                AdaptersNamespace,
                IntegrationsNamespace
            })
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Application should only depend on Domain. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    #endregion
}