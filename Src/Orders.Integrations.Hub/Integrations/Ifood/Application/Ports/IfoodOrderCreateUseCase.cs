using Orders.Integrations.Hub.Core..Domain.Contracts;
using Orders.Integrations.Hub.Core..Domain.ValueObjects;
using Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.Ifood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Ports;

public class IfoodOrderCreateUseCase(
    IInternalClient Client,
    IIFoodClient iFoodClient
) : IOrderCreateUseCase<IfoodWebhookRequest> {
    public async Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest requestOrder)
    {
        IfoodOrder ifoodOrder = await iFoodClient.GetOrderDetails(requestOrder.OrderId);

        ResponseWrapper<IntegrationResponse> integrationWrapper = await Client.GetIntegrationByExternalId(requestOrder.MerchantId);
        IntegrationResponse integration = integrationWrapper.Data;

        int companyId = integration.CompanyId ?? 0;

        await new CreateOrderEvent(
            Order: ifoodOrder.ToOrder(companyId),
            SalesChannel: OrderSalesChannel.IFOOD
        ).PublishAsync();


        if (integration.Resolve().AutoAccept)
        {
            await new SendNotificationEvent(
                Message: requestOrder.FromIfood(OrderEventType.CONFIRMED),
                TopicArn: null
            ).PublishAsync();
        }

        return requestOrder;
    }
}