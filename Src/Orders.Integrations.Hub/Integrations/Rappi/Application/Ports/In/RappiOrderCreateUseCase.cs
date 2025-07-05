using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;

using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.In;

public class RappiOrderCreateUseCase(
) : IOrderCreateUseCase<RappiOrder> {
    public async Task<RappiOrder> ExecuteAsync(RappiOrder requestOrder)
    {
        const int companyId = 0;

        await new CreateOrderEvent(
            Order: requestOrder.ToOrder(companyId),
            SalesChannel: OrderSalesChannel.RAPPI
        ).PublishAsync();

        return requestOrder;
    }
}