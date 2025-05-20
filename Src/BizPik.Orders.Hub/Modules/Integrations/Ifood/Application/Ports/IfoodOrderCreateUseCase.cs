using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Extensions;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderCreateUseCase(
    IBizPikMonolithClient bizPikClient,
    IIFoodClient iFoodClient
) : IOrderCreateUseCase<IfoodWebhookRequest> {
    public async Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest requestOrder)
    {
        IfoodOrder ifoodOrder = await iFoodClient.GetOrderDetails(requestOrder.OrderId);

        BizPikResponseWrapper<BizPikIntegrationResponse> integrationWrapper = await bizPikClient.GetIntegrationByExternalId(requestOrder.MerchantId);
        BizPikIntegrationResponse integration = integrationWrapper.Data;

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