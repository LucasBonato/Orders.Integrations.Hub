using Orders.Integrations.Hub.Integrations.Common.Serialization;

namespace Orders.Integrations.Hub.UnitTests.Serialization;

public class CommonJsonSerializerTests
{
    private static readonly CommonJsonSerializer Sut = new();

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
    public void Deserialize_ShouldMatchCoreSerializer()
    {
        // CommonJsonSerializer uses same settings as CoreJsonSerializer
        CommonDto? result = Sut.Deserialize<CommonDto>("""{"id":"abc","active":true}""");

        Assert.NotNull(result);
        Assert.Equal("abc", result.Id);
        Assert.True(result.Active);
    }

    [Fact]
    public void RoundTrip_ShouldPreserveValues()
    {
        CommonDto original = new("xyz", false);

        string json = Sut.Serialize(original);
        CommonDto? result = Sut.Deserialize<CommonDto>(json);

        Assert.NotNull(result);
        Assert.Equal(original.Id, result.Id);
        Assert.Equal(original.Active, result.Active);
    }

    private record CommonDto(string Id, bool Active);
}