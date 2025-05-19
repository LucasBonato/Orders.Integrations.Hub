using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Response;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Ports;

public class IfoodOrderGetCancellationReasonUseCase(
    IIFoodClient iFoodClient
) : IOrderGetCancellationReasonUseCase {
    public async Task<List<CancellationReasonsResponse>> ExecuteAsync(string? externalOrderId)
    {
        if (string.IsNullOrEmpty(externalOrderId)) {
            throw new ArgumentNullException(nameof(externalOrderId));
        }

        var ifoodCancellationReasons = await iFoodClient.GetCancellationReasons(externalOrderId);
        return ifoodCancellationReasons
            .Select(reason => new CancellationReasonsResponse(
                Code: Convert.ToInt32(reason.CancelCodeId),
                Name: Enum.GetName(typeof(IfoodCancellationReasons), Convert.ToInt32(reason.CancelCodeId)),
                Description: reason.Description
            ))
            .ToList();
    }
}