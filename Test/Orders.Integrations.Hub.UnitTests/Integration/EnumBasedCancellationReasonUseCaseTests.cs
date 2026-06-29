using Orders.Integrations.Hub.Core.Application.DTOs.Response;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Integrations.Common.Application;

namespace Orders.Integrations.Hub.UnitTests.Integration;

public class EnumBasedCancellationReasonUseCaseTests
{
    private enum TestCancellationReasons
    {
        ReasonA = 1,
        ReasonB = 2,
    }

    private sealed class TestUseCase : EnumBasedCancellationReasonUseCase<TestCancellationReasons> { }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnAllEnumValues()
    {
        IOrderGetCancellationReasonUseCase sut = new TestUseCase();

        List<CancellationReasonsResponse> result = await sut.ExecuteAsync("order-123");

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldMapCodeCorrectly()
    {
        IOrderGetCancellationReasonUseCase sut = new TestUseCase();

        List<CancellationReasonsResponse> result = await sut.ExecuteAsync("order-123");

        Assert.Contains(result, reason => reason is { Code: 1, Name: nameof(TestCancellationReasons.ReasonA) });
        Assert.Contains(result, reason => reason is { Code: 2, Name: nameof(TestCancellationReasons.ReasonB) });
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnDescription()
    {
        IOrderGetCancellationReasonUseCase sut = new TestUseCase();

        List<CancellationReasonsResponse> result = await sut.ExecuteAsync("order-123");

        Assert.All(result, reason => Assert.False(string.IsNullOrWhiteSpace(reason.Description)));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldIgnoreExternalOrderId()
    {
        IOrderGetCancellationReasonUseCase sut = new TestUseCase();

        List<CancellationReasonsResponse> result1 = await sut.ExecuteAsync("order-1");
        List<CancellationReasonsResponse> result2 = await sut.ExecuteAsync(null);
        List<CancellationReasonsResponse> result3 = await sut.ExecuteAsync("order-2");

        Assert.Equal(result1.Count, result2.Count);
        Assert.Equal(result1.Count, result3.Count);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleEmptyEnum()
    {
        IOrderGetCancellationReasonUseCase sut = new EmptyUseCase();

        List<CancellationReasonsResponse> result = await sut.ExecuteAsync("order-123");

        Assert.Empty(result);
    }

    private enum EmptyReasons { }
    private sealed class EmptyUseCase : EnumBasedCancellationReasonUseCase<EmptyReasons> { }
}