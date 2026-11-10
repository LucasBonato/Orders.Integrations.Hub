using Microsoft.Extensions.DependencyInjection;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures.Collections;
using Orders.Integrations.Hub.TestCommon.Fakers.Commands;

namespace Orders.Integrations.Hub.IntegrationTests.Features.OrderStatus;

[Collection(IntegrationTestCollection.Name)]
public sealed class UpdateOrderStatusCommandHandlerTests(
    IntegrationHostFixture fixture
) : IntegrationTestBase(fixture) {
    [Fact]
    public async Task Publish_ShouldPatchOrderThroughOrdersService() {
        // Arrange
        await Host.WireMock.OrdersApi.StubPatchOrderAsync(cancellationToken: TestContext.Current.CancellationToken);
        UpdateOrderStatusCommand command = new(
            new OrderUpdateFaker().Generate(),
            IntegrationKey.From("IFOOD")
        );

        // Act
        using IServiceScope scope = Host.Services.CreateScope();
        ICommandDispatcher dispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        await dispatcher.DispatchAsync(command, TestContext.Current.CancellationToken);

        // Assert
        await Host.WireMock.OrdersApi.WaitForPatchOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, await Host.WireMock.OrdersApi.RequestCountAsync("/Orders", TestContext.Current.CancellationToken));
    }
}
