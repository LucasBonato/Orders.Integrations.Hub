using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.Out;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Ports.Out;

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