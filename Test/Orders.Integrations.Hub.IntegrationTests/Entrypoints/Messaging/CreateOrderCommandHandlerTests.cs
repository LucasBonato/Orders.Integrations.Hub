using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.TestCommon.Fakers.Commands;

namespace Orders.Integrations.Hub.IntegrationTests.Entrypoints.Messaging;

[Collection(IntegrationTestCollection.Name)]
public sealed class CreateOrderCommandHandlerTests(
    TestInfrastructure infrastructure
) : IntegrationTestBase(infrastructure) {
    [Fact]
    public async Task Publish_ShouldPostOrderToOrdersService() {
        // Arrange
        Host.WireMock.OrdersApi.StubCreateOrder();
        CreateOrderCommand command = new CreateOrderCommandFaker().Generate();

        // Act
        using IServiceScope scope = Host.Services.CreateScope();
        IPublishEndpoint bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        await bus.Publish(command, TestContext.Current.CancellationToken);

        // Assert
        await Host.WireMock.OrdersApi.WaitForCreateOrderAsync(TestContext.Current.CancellationToken);
        string body = Host.WireMock.OrdersApi.LastRequestBody("/Orders");
        Assert.Contains(command.Order.OrderId, body, StringComparison.Ordinal);
    }
}
