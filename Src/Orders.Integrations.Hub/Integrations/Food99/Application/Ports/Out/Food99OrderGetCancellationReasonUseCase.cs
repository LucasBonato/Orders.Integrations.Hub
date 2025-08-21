using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.Out;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Ports.Out;

public class Food99OrderGetCancellationReasonUseCase : IOrderGetCancellationReasonUseCase
{
    public async Task<List<CancellationReasonsResponse>> ExecuteAsync(string? externalOrderId)
    {
        return await Task.Run(() => Enum.GetValues<Food99OrderCancelType>()
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

        string? raw = value.ToString();

        if (string.IsNullOrWhiteSpace(raw))
            return string.Empty;

        return string
            .Concat(raw
                .Select(c => Char.IsUpper(c)
                    ? $" {c}"
                    : c.ToString()
                )
            )
            .TrimStart();
    }
}