using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderGetCancellationReasonUseCase(
    IIFoodClient iFoodClient
) : IOrderGetCancellationReasonUseCase {
    public Task<List<string>> ExecuteAsync(string orderId)
    {
        return Task.FromResult(Enum.GetValues<IfoodCancellationReasons>().Select(value => value.ToString()).ToList());
    }
}