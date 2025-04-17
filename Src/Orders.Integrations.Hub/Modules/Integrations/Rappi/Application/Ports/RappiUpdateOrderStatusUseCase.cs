using Orders.Integrations.Hub.Modules.Core..Domain.Contracts;
using Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Modules.Integrations.Rappi.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Integrations.Rappi.Domain.Entity;

using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Integrations.Rappi.Application.Ports;

public class RappiUpdateOrderStatusUseCase(
    ILogger<RappiUpdateOrderStatusUseCase> logger,
    IInternalClient Client,
    IRappiClient rappiClient
) : IUpdateOrderStatusUseCase<RappiOrder> {
    public async Task<RappiOrder> ExecuteAsync(RappiOrder requestOrder)
    {
        ResponseWrapper<IntegrationResponse> integrationWrapper = await Client.GetIntegrationByExternalId(requestOrder.Store.ExternalId, AppEnv..MONOLITH.API_KEYS.COMPANIES_INTEGRATIONS.NotNull());
        IntegrationResponse integration = integrationWrapper.Data;

        int companyId = integration.CompanyId ?? 0;

        await new CreateOrderEvent()
        {
            Order = requestOrder.ToOrder(companyId),
            SalesChannel = OrderSalesChannel.RAPPI
        }.PublishAsync();
    }
}