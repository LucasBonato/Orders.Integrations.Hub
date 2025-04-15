using Orders.Integrations.Hub.Modules.Core..Domain.Contracts;
using Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Modules.Integrations.Common.Application;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Extensions;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodCreateOrderUseCase(
    ILogger<IfoodCreateOrderUseCase> logger,
    IInternalClient Client,
    IIFoodClient iFoodClient
) : ICreateOrderUseCase<IfoodWebhookRequest> {
    public async Task<IfoodWebhookRequest> ExecuteAsync(IfoodWebhookRequest requestOrder)
    {
        IfoodOrder ifoodOrder = await iFoodClient.GetOrderDetails(requestOrder.OrderId);

        ResponseWrapper<IntegrationResponse> integrationWrapper = await Client.GetIntegrationByExternalId(requestOrder.MerchantId ?? string.Empty, AppEnv..MONOLITH.API_KEYS.COMPANIES_INTEGRATIONS.NotNull());
        IntegrationResponse integration = integrationWrapper.Data;

        int companyId = integration.CompanyId ?? 0;

        await new CreateOrderEvent() {
            Order = ifoodOrder.ToOrder(companyId),
            SalesChannel = OrderSalesChannel.IFOOD,
        }.PublishAsync();


        if (integration.Resolve().AutoAccept)
        {
            await new SendNotificationEvent() {
                Message = requestOrder.FromIfood(),
            }.PublishAsync();
        }

        return await Task.Run(() => requestOrder);
    }
}