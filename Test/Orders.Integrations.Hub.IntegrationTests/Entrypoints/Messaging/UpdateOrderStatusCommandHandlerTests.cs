using MassTransit;
using Microsoft.Extensions.DependencyInjection;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.TestCommon.Fakers.Commands;

namespace Orders.Integrations.Hub.IntegrationTests.Entrypoints.Messaging;

[Collection(IntegrationTestCollection.Name)]
public sealed class UpdateOrderStatusCommandHandlerTests(
    TestInfrastructure infrastructure
) : IntegrationTestBase(infrastructure) {
    [Fact]
    public async Task Publish_ShouldPatchOrderThroughOrdersService() {
        // Arrange
        Host.WireMock.OrdersApi.StubPatchOrder();
        UpdateOrderStatusCommand command = new(
            new OrderUpdateFaker().Generate(),
            IntegrationKey.From("IFOOD")
        );
        IBusControl busControl = Host.Services.GetRequiredService<IBusControl>();
        BusHandle busHandle = await busControl.StartAsync(TestContext.Current.CancellationToken);
        await busHandle.Ready;

        // Act
        using (IServiceScope scope = Host.Services.CreateScope())
        {
            IPublishEndpoint bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            await bus.Publish(command, TestContext.Current.CancellationToken);
        }

        // Assert
        await Host.WireMock.OrdersApi.WaitForPatchOrderAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, Host.WireMock.OrdersApi.RequestCount("/Orders"));
    }
}
