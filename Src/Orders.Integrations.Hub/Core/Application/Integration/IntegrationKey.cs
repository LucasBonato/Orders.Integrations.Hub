namespace Orders.Integrations.Hub.Core.Application.Integration;

public sealed record IntegrationKey {
    public string Value { get; }

    private IntegrationKey(string value) => Value = value;

    public static IntegrationKey From(string value) => new(value.Trim().ToUpperInvariant());

    public static IntegrationKey Nothing() => From(string.Empty);
}