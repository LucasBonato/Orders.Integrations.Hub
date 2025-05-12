using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderGetCancellationReasonUseCase(
    IIFoodClient iFoodClient
) : IOrderGetCancellationReasonUseCase {
    public Task<List<string>> ExecuteAsync(string orderId)
    {
        return Task.FromResult(Enum.GetValues<IfoodCancellationReasons>().Select(value => value.ToString()).ToList());
    }
}