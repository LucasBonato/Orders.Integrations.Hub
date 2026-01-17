using System.Reflection;

using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.IFood;

namespace Orders.Integrations.Hub.UnitTests.Integrations;

public class IntegrationKeyValidationTests
{
    [Theory]
    [InlineData("IFOOD")]
    [InlineData("RAPPI")]
    [InlineData("FOOD99")]
    [InlineData("KEETA")]
    public void ValidateRawValue_WithValidUppercaseValue_DoesNotThrowException(string value)
    {
        // Arrange
        // (value provided by theory data)

        // Act
        var exception = Record.Exception(() =>
            IntegrationKeyValidator.ValidateRawValue(value)
        );

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("ifood")]
    [InlineData("Ifood")]
    [InlineData(" IFOOD")]
    [InlineData("IFOOD ")]
    [InlineData("")]
    public void ValidateRawValue_WithInvalidValue_ThrowsInvalidOperationException(string value)
    {
        // Arrange
        // (value provided by theory data)

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            IntegrationKeyValidator.ValidateRawValue(value)
        );
    }

    [Fact]
    public void GetIntegrationKeyTypes_InIntegrationsNamespace_AllHaveIntegrationKeyDefinitionAttribute()
    {
        // Arrange
        var assembly = typeof(IFoodIntegrationKey).Assembly;

        // Act
        List<Type> typesWithoutAttribute = assembly
            .GetTypes()
            .Where(type =>
                type.Name.EndsWith("IntegrationKey") &&
                type != typeof(IntegrationKey) &&
                type.GetCustomAttribute<IntegrationKeyDefinitionAttribute>() is null &&
                type.Namespace?.StartsWith("Orders.Integrations.Hub.Integrations.") is true
            )
            .ToList();

        // Assert
        Assert.Empty(typesWithoutAttribute);
    }

    [Fact]
    public void GetIntegrationKeyTypes_WithIntegrationKeyDefinitionAttribute_AllHaveValueField()
    {
        // Arrange
        var assembly = typeof(IFoodIntegrationKey).Assembly;
        var integrationKeyTypes = assembly
            .GetTypes()
            .Where(type => type.GetCustomAttribute<IntegrationKeyDefinitionAttribute>() is not null)
            .ToList();

        // Act & Assert
        Assert.NotEmpty(integrationKeyTypes); // Ensure we found at least one

        foreach (var type in integrationKeyTypes)
        {
            // Act
            var valueField = type.GetField("Value", BindingFlags.Public | BindingFlags.Static);

            // Assert
            Assert.NotNull(valueField);
        }
    }

    [Fact]
    public void GetIntegrationKeyTypes_WithIntegrationKeyDefinitionAttribute_AllValuesAreNormalized()
    {
        // Arrange
        var assembly = typeof(IFoodIntegrationKey).Assembly;
        var integrationKeyTypes = assembly
            .GetTypes()
            .Where(type => type.GetCustomAttribute<IntegrationKeyDefinitionAttribute>() is not null)
            .ToList();

        Assert.NotEmpty(integrationKeyTypes); // Precondition

        // Act & Assert
        foreach (var type in integrationKeyTypes)
        {
            // Arrange (per iteration)
            var valueField = type.GetField("Value", BindingFlags.Public | BindingFlags.Static);
            Assert.NotNull(valueField); // Precondition

            // Act
            var value = (string)valueField.GetValue(null)!;
            var normalizedValue = value.Trim().ToUpperInvariant();

            // Assert
            Assert.Equal(normalizedValue, value);
        }
    }
}