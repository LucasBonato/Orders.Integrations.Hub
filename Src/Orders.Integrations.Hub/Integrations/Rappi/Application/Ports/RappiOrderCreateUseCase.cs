using Orders.Integrations.Hub.Core.Orders.Domain.Contracts;
using Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

using FastEndpoints;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports;

public class RappiOrderCreateUseCase(
    IInternalClient Client
) : IOrderCreateUseCase<RappiOrder> {
    public async Task<RappiOrder> ExecuteAsync(RappiOrder requestOrder)
    {
        ResponseWrapper<IntegrationResponse> integrationWrapper = await Client.GetIntegrationByExternalId(requestOrder.Store.ExternalId);
        IntegrationResponse integration = integrationWrapper.Data;

        int companyId = integration.CompanyId ?? 0;

        await new CreateOrderEvent(
            Order: requestOrder.ToOrder(companyId),
            SalesChannel: OrderSalesChannel.RAPPI
        ).PublishAsync();

        return requestOrder;
    }
}