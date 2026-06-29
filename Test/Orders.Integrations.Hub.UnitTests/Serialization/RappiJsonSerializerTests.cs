using Orders.Integrations.Hub.Integrations.Rappi.Application.Handlers;

namespace Orders.Integrations.Hub.UnitTests.Serialization;

public class RappiJsonSerializerTests
{
    private static readonly RappiJsonSerializer Sut = new();

    private enum TestEnum { TakenVisibleOrder }

    [Fact]
    public void Serialize_ShouldUseSnakeCaseLower()
    {
        string json = Sut.Serialize(new { SomeProperty = "value" });

        Assert.Contains("some_property", json);
    }

    [Fact]
    public void Serialize_ShouldUseSnakeCaseUpperForEnums()
    {
        string json = Sut.Serialize(new { Event = TestEnum.TakenVisibleOrder });

        Assert.Contains("TAKEN_VISIBLE_ORDER", json);
    }

    [Fact]
    public void Deserialize_ShouldHandleSnakeCase()
    {
        RappiDto? result = Sut.Deserialize<RappiDto>("""{"order_id":"ord-1","total_order":1550}""");

        Assert.NotNull(result);
        Assert.Equal("ord-1", result.OrderId);
        Assert.Equal(1550, result.TotalOrder);
    }

    [Fact]
    public void RoundTrip_ShouldPreserveValues()
    {
        RappiDto original = new("ord-2", 2500);

        string json = Sut.Serialize(original);
        RappiDto? result = Sut.Deserialize<RappiDto>(json);

        Assert.NotNull(result);
        Assert.Equal(original.OrderId, result.OrderId);
        Assert.Equal(original.TotalOrder, result.TotalOrder);
    }

    private record RappiDto(string OrderId, int TotalOrder);
}