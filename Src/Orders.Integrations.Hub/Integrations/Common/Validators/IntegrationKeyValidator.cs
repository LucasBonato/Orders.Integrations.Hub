using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;

namespace Orders.Integrations.Hub.Integrations.Common.Validators;

public static class IntegrationKeyValidator {
    public static void ValidateRawValue(string value) {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Integration key value cannot be null or whitespace.");

        string normalizedValue = IntegrationKey.From(value);

        if (normalizedValue != value)
            throw new InvalidOperationException($"Integration key value must be uppercase with no whitespace. Expected: {normalizedValue}, Got {value}.");
    }
}