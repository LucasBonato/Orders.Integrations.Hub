namespace Orders.Integrations.Hub.Core.Domain.ValueObjects;

public sealed record IntegrationKey {
    public string Value { get; }

    private IntegrationKey(string value) {
        if (value is null)
            throw new InvalidOperationException("Integration Key cannot be null.");

        Value = value.Trim().ToUpperInvariant();
    }

    public static IntegrationKey From(string value) => new(value);

    public static IntegrationKey Nothing() => From(string.Empty);

    public override string ToString() => Value;

    public static implicit operator string(IntegrationKey key) => key.Value;
}