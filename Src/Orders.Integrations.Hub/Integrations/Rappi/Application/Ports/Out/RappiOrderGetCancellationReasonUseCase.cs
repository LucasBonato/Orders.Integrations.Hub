using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.Out;

public class RappiOrderGetCancellationReasonUseCase : IOrderGetCancellationReasonUseCase
{
    public async Task<List<CancellationReasonsResponse>> ExecuteAsync(string? externalOrderId)
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