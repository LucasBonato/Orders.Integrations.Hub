using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Contracts;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderGetCancellationReasonUseCase(
    IIFoodClient iFoodClient
) : IOrderGetCancellationReasonUseCase<String> {
    public async Task<IReadOnlyList<string>> ExecuteAsync(OrderCancellationReasonRequest integrationOrder)
    {
        await iFoodClient.GetCancellationReasons(orderId: integrationOrder.OrderId);
        throw new NotImplementedException();
    }
}