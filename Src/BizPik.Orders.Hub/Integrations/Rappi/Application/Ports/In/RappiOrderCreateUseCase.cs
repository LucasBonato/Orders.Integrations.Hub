using BizPik.Orders.Hub.Core.Domain.Contracts;
using BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.BizPik;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;
using BizPik.Orders.Hub.Integrations.Rappi.Application.Extensions;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.Entity;

using FastEndpoints;

namespace BizPik.Orders.Hub.Integrations.Rappi.Application.Ports.In;

public class RappiOrderCreateUseCase(
    IBizPikMonolithClient bizPikClient
) : IOrderCreateUseCase<RappiOrder> {
    public async Task<RappiOrder> ExecuteAsync(RappiOrder requestOrder)
    {
        BizPikResponseWrapper<BizPikIntegrationResponse> integrationWrapper = await bizPikClient.GetIntegrationByExternalId(requestOrder.Store.ExternalId);
        BizPikIntegrationResponse integration = integrationWrapper.Data;

        int companyId = integration.CompanyId ?? 0;

        await new CreateOrderEvent(
            Order: requestOrder.ToOrder(companyId),
            SalesChannel: OrderSalesChannel.RAPPI
        ).PublishAsync();

        return requestOrder;
    }
}