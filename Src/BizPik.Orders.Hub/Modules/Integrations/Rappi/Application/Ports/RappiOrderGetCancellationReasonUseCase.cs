using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs.Response;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Ports;

public class RappiOrderGetCancellationReasonUseCase : IOrderGetCancellationReasonUseCase
{
    public async Task<List<CancellationReasonsResponse>> ExecuteAsync(string? orderId)
    {
        return await Task.Run(() => Enum.GetValues<RappiOrderCancelType>()
            .Select(value => new CancellationReasonsResponse(
                Code: (int)value,
                Name: value.ToString(),
                Description: ToPrettyString(value)
            ))
            .ToList());
    }

    private static string ToPrettyString(object? value)
    {
        if (value == null) return string.Empty;

        string raw = value.ToString()?.Replace('_', ' ').Trim().ToLowerInvariant() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(raw))
            return string.Empty;

        return char.ToUpper(raw[0]) + raw[1..] + ".";
    }
}