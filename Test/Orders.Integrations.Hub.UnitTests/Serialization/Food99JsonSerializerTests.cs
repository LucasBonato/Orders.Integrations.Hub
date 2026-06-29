using Orders.Integrations.Hub.Integrations.Food99.Application.Handlers;

namespace Orders.Integrations.Hub.UnitTests.Serialization;

public class Food99JsonSerializerTests
{
    private static readonly Food99JsonSerializer Sut = new();

    private enum TestEnum { OrderNew }

    [Fact]
    public void Serialize_ShouldUseSnakeCaseLower()
    {
        string json = Sut.Serialize(new { SomeProperty = "value" });

        Assert.Contains("some_property", json);
    }

    [Fact]
    public void Serialize_ShouldUseCamelCaseForEnums()
    {
        string json = Sut.Serialize(new { Type = TestEnum.OrderNew });

        Assert.Contains("orderNew", json);
    }

    [Fact]
    public void Deserialize_ShouldHandleSnakeCase()
    {
        Food99Dto? result = Sut.Deserialize<Food99Dto>("""{"app_id":123,"order_id":"ord-1"}""");

        Assert.NotNull(result);
        Assert.Equal(123, result.AppId);
        Assert.Equal("ord-1", result.OrderId);
    }

    [Fact]
    public void RoundTrip_ShouldPreserveValues()
    {
        Food99Dto original = new(456, "ord-2");

        string json = Sut.Serialize(original);
        Food99Dto? result = Sut.Deserialize<Food99Dto>(json);

        Assert.NotNull(result);
        Assert.Equal(original.AppId, result.AppId);
        Assert.Equal(original.OrderId, result.OrderId);
    }

    private record Food99Dto(int AppId, string OrderId);
}