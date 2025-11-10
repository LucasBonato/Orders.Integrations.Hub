using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.In;

public class RappiOrderCreateUseCase(
    IIntegrationContext integrationContext
) : IOrderCreateUseCase<RappiOrder> {
    public async Task<RappiOrder> ExecuteAsync(RappiOrder request)
    {
        string tenantId = integrationContext.Integration!.TenantId?? string.Empty;

        await new CreateOrderEvent(
            Order: request.ToOrder(tenantId),
            SalesChannel: OrderSalesChannel.RAPPI
        ).PublishAsync();

        if (integrationContext.Integration.AutoAccept)
        {
            await new SendNotificationEvent(
                Message: request.FromRappi(OrderEventType.CONFIRMED),
                TopicArn: null
            ).PublishAsync();
        }

        return request;
    }
}