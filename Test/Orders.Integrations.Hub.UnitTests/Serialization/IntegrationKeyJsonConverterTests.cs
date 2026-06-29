using System.Text.Json;

using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.Core.Infrastructure.Serialization;

namespace Orders.Integrations.Hub.UnitTests.Serialization;

public class IntegrationKeyJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new IntegrationKeyJsonConverter() }
    };

    [Fact]
    public void Read_ShouldReturnIntegrationKey()
    {
        const string json = "\"IFOOD\"";

        IntegrationKey? key = JsonSerializer.Deserialize<IntegrationKey>(json, Options);

        Assert.NotNull(key);
        Assert.Equal("IFOOD", key.Value);
    }

    [Fact]
    public void Read_ShouldNormalizeToUpper()
    {
        const string json = "\"ifood\"";

        IntegrationKey? key = JsonSerializer.Deserialize<IntegrationKey>(json, Options);

        Assert.NotNull(key);
        Assert.Equal("IFOOD", key.Value);
    }

    [Fact]
    public void Read_ShouldReturnNull_WhenJsonNull()
    {
        IntegrationKey? result = JsonSerializer.Deserialize<IntegrationKey>("null", Options);

        Assert.Null(result);
    }

    [Fact]
    public void Write_ShouldOutputValue()
    {
        IntegrationKey key = IntegrationKey.From("RAPPI");

        string json = JsonSerializer.Serialize(key, Options);

        Assert.Equal("\"RAPPI\"", json);
    }

    [Fact]
    public void RoundTrip_ShouldPreserveKey()
    {
        IntegrationKey original = IntegrationKey.From("FOOD99");

        string json = JsonSerializer.Serialize(original, Options);
        IntegrationKey? result = JsonSerializer.Deserialize<IntegrationKey>(json, Options);

        Assert.NotNull(result);
        Assert.Equal(original, result);
    }

    [Fact]
    public void RoundTrip_InWrapperObject()
    {
        Wrapper original = new(IntegrationKey.From("IFOOD"));

        string json = JsonSerializer.Serialize(original, Options);
        Wrapper? result = JsonSerializer.Deserialize<Wrapper>(json, Options);

        Assert.NotNull(result);
        Assert.Equal(original.Key, result.Key);
    }

    public record Wrapper(IntegrationKey Key);
}