using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderGetCancellationReasonUseCase(
    IIFoodClient iFoodClient
) : IOrderGetCancellationReasonUseCase<String> {
    public async Task<IReadOnlyList<string>> ExecuteAsync(OrderCancellationReasonRequest integrationOrder)
    {
        await iFoodClient.GetCancellationReasons(orderId: integrationOrder.OrderId);
        throw new NotImplementedException();
    }
}