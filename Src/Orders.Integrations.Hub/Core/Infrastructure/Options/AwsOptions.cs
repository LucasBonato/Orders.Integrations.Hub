namespace Orders.Integrations.Hub.Core.Infrastructure.Options;

public sealed record AwsProviderOptions(
    string? Profile,
    string? ServiceUrlOverride,
    bool ForcePathStyle = false,
    string Region = "us-east-1"
) {
    public const string SectionName = "Providers:Aws";

    public static AwsProviderOptions Create() => new(
        Profile: null,
        ServiceUrlOverride: null
    );
    
    public bool HasEndpointOverride => !string.IsNullOrWhiteSpace(ServiceUrlOverride);
}