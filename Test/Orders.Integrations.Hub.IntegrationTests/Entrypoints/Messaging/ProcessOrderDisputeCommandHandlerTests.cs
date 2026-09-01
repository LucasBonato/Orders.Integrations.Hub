using MassTransit;
using Microsoft.Extensions.DependencyInjection;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.TestCommon.Fakers.Order;

namespace Orders.Integrations.Hub.IntegrationTests.Entrypoints.Messaging;

[Collection(IntegrationTestCollection.Name)]
public sealed class ProcessOrderDisputeCommandHandlerTests(
    TestInfrastructure infrastructure
) : IntegrationTestBase(infrastructure) {
    [Fact]
    public async Task Publish_ShouldPatchOrderDisputeThroughOrdersService()
    {
        // Arrange
        Host.WireMock.OrdersApi.StubPatchOrderDispute();
        ProcessOrderDisputeCommand command = new(
            ExternalOrderId: "external-order-1",
            Integration: IntegrationKey.From("IFOOD"),
            OrderDispute: new OrderDisputeFaker().Generate(),
            Type: OrderEventType.DISPUTE_STARTED);

        // Act
        using (IServiceScope scope = Host.Services.CreateScope())
        {
            IPublishEndpoint bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            await bus.Publish(command, TestContext.Current.CancellationToken);
        }

        // Assert
        await Host.WireMock.OrdersApi.WaitForPatchOrderDisputeAsync(TestContext.Current.CancellationToken);
        Assert.Equal(1, Host.WireMock.OrdersApi.RequestCount("/Orders/dispute"));
    }
}
