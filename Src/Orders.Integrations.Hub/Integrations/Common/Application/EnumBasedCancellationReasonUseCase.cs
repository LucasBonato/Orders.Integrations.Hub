using Orders.Integrations.Hub.Core.Application.DTOs.Response;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.Out;
using Orders.Integrations.Hub.Integrations.Common.Extensions;

namespace Orders.Integrations.Hub.Integrations.Common.Application;

public abstract class EnumBasedCancellationReasonUseCase<TEnum> : IOrderGetCancellationReasonUseCase where TEnum : struct, Enum
{
    public async Task<List<CancellationReasonsResponse>> ExecuteAsync(string? externalOrderId)
    {
        return await Task.FromResult(Enum.GetValues<TEnum>()
            .Select(value => new CancellationReasonsResponse(
                Code: Convert.ToInt32(value),
                Name: value.ToString(),
                Description: value.ToPrettyString()
            ))
            .ToList());
    }
}