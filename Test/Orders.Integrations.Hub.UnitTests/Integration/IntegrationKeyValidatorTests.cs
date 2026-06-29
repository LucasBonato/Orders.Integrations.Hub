using Orders.Integrations.Hub.Integrations.Common.Validators;

namespace Orders.Integrations.Hub.UnitTests.Integration;

public class IntegrationKeyValidatorTests
{
    [Fact]
    public void ValidateRawValue_ShouldThrow_WhenValueIsNull()
    {
        Assert.Throws<InvalidOperationException>(() =>
            IntegrationKeyValidator.ValidateRawValue(null!));
    }

    [Fact]
    public void ValidateRawValue_ShouldThrow_WhenValueIsWhitespace()
    {
        Assert.Throws<InvalidOperationException>(() =>
            IntegrationKeyValidator.ValidateRawValue("   "));
    }

    [Fact]
    public void ValidateRawValue_ShouldThrow_WhenValueIsEmpty()
    {
        Assert.Throws<InvalidOperationException>(() =>
            IntegrationKeyValidator.ValidateRawValue(string.Empty));
    }

    [Fact]
    public void ValidateRawValue_ShouldThrow_WhenValueIsNotUppercase()
    {
        Assert.Throws<InvalidOperationException>(() =>
            IntegrationKeyValidator.ValidateRawValue("ifood"));
    }

    [Fact]
    public void ValidateRawValue_ShouldThrow_WhenValueHasWhitespace()
    {
        Assert.Throws<InvalidOperationException>(() =>
            IntegrationKeyValidator.ValidateRawValue("IFOOD "));
    }

    [Fact]
    public void ValidateRawValue_ShouldNotThrow_WhenValueIsValid()
    {
        var exception = Record.Exception(() =>
            IntegrationKeyValidator.ValidateRawValue("IFOOD"));

        Assert.Null(exception);
    }
}