using Orders.Integrations.Hub.Core.Application.DTOs.Response;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Ports.Out;

public class IFoodOrderGetCancellationReasonUseCase(
    IIFoodClient iFoodClient
) : IOrderGetCancellationReasonUseCase {
    public async Task<List<CancellationReasonsResponse>> ExecuteAsync(string? externalOrderId)
    {
        if (string.IsNullOrEmpty(externalOrderId))
            throw new ArgumentNullException(nameof(externalOrderId));

        var ifoodCancellationReasons = await iFoodClient.GetCancellationReasons(externalOrderId);

        return ifoodCancellationReasons
            .Select(reason => new CancellationReasonsResponse(
                Code: Convert.ToInt32(reason.CancelCodeId),
                Name: Enum.GetName(typeof(IFoodCancellationReasons), Convert.ToInt32(reason.CancelCodeId)),
                Description: reason.Description
            ))
            .ToList();
    }
}