using Microsoft.Extensions.DependencyInjection;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures.Collections;
using Orders.Integrations.Hub.TestCommon.Fakers.Commands;

namespace Orders.Integrations.Hub.IntegrationTests.Features.CreateOrder;

[Collection(IntegrationTestCollection.Name)]
public sealed class CreateOrderCommandHandlerTests(
    IntegrationHostFixture fixture
) : IntegrationTestBase(fixture) {
    [Fact]
    public async Task Publish_ShouldPostOrderToOrdersService() {
        // Arrange
        Host.WireMock.OrdersApi.StubCreateOrder();
        CreateOrderCommand command = new CreateOrderCommandFaker().Generate();

        // Act
        using IServiceScope scope = Host.Services.CreateScope();
        ICommandDispatcher dispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        await dispatcher.DispatchAsync(command, TestContext.Current.CancellationToken);

        // Assert
        await Host.WireMock.OrdersApi.WaitForCreateOrderAsync(TestContext.Current.CancellationToken);
        string body = Host.WireMock.OrdersApi.LastRequestBody("/Orders");
        Assert.Contains(command.Order.OrderId, body, StringComparison.Ordinal);
    }
}
