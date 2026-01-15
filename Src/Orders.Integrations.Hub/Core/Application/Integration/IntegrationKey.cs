using System.Diagnostics.CodeAnalysis;

namespace Orders.Integrations.Hub.Core.Application.Integration;

public sealed record IntegrationKey : IParsable<IntegrationKey> {
    public string Value { get; }

    private IntegrationKey(string value) => Value = value;

    public static IntegrationKey From(string value) => new(value.Trim().ToUpperInvariant());

    public static IntegrationKey Nothing() => From(string.Empty);

    public static implicit operator string(IntegrationKey key) => key.Value;

    public static IntegrationKey Parse(string s, IFormatProvider? provider)
    {
        return string.IsNullOrWhiteSpace(s)
            ? throw new FormatException("Integration key cannot be null or whitespace.")
            : From(s);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out IntegrationKey result
    ) {
        if (string.IsNullOrWhiteSpace(s)) {
            result = Nothing();
            return false;
        }

        try {
            result = From(s);
            return true;
        }
        catch {
            result = Nothing();
            return false;
        }
    }
}