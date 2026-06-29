using Orders.Integrations.Hub.Core.Infrastructure.Serialization;

namespace Orders.Integrations.Hub.UnitTests.Serialization;

public class CoreJsonSerializerTests
{
    private static readonly CoreJsonSerializer Sut = new();

    private enum TestEnum { FirstValue }

    [Fact]
    public void Serialize_ShouldUseCamelCase()
    {
        string json = Sut.Serialize(new { SomeProperty = "value" });

        Assert.Contains("someProperty", json);
    }

    [Fact]
    public void Serialize_ShouldUseSnakeCaseUpperForEnums()
    {
        string json = Sut.Serialize(new { Status = TestEnum.FirstValue });

        Assert.Contains("FIRST_VALUE", json);
    }

    [Fact]
    public void Deserialize_ShouldHandleCamelCase()
    {
        TestDto? result = Sut.Deserialize<TestDto>("""{"name":"test","count":42}""");

        Assert.NotNull(result);
        Assert.Equal("test", result.Name);
        Assert.Equal(42, result.Count);
    }

    [Fact]
    public void RoundTrip_ShouldPreserveValues()
    {
        TestDto original = new("roundtrip", 99);

        string json = Sut.Serialize(original);
        TestDto? result = Sut.Deserialize<TestDto>(json);

        Assert.NotNull(result);
        Assert.Equal(original.Name, result.Name);
        Assert.Equal(original.Count, result.Count);
    }

    [Fact]
    public void Deserialize_ShouldReturnDefault_WhenNull()
    {
        TestDto? result = Sut.Deserialize<TestDto>("null");

        Assert.Null(result);
    }

    private record TestDto(string? Name, int Count);
}