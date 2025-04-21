using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Extensions;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;

using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Ports;

public class RappiOrderCreateUseCase(
    ILogger<RappiOrderCreateUseCase> logger,
    IBizPikMonolithClient bizPikClient
) : IOrderCreateUseCase<RappiOrder> {
    public async Task<RappiOrder> ExecuteAsync(RappiOrder requestOrder)
    {
        BizPikResponseWrapper<BizPikIntegrationResponse> integrationWrapper = await bizPikClient.GetIntegrationByExternalId(requestOrder.Store.ExternalId, AppEnv.BIZPIK.MONOLITH.API_KEYS.COMPANIES_INTEGRATIONS.NotNull());
        BizPikIntegrationResponse integration = integrationWrapper.Data;

        int companyId = integration.CompanyId ?? 0;

        await new CreateOrderEvent(
            Order: requestOrder.ToOrder(companyId),
            SalesChannel: OrderSalesChannel.RAPPI
        ).PublishAsync();

        throw new NotImplementedException();
    }
}