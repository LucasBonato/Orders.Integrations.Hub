using OpenTelemetry.Exporter;

namespace Orders.Integrations.Hub.Core.Infrastructure.Options;

public sealed record OpenTelemetryOptions(
    string ServiceName,
    Uri Endpoint,
    OtlpExportProtocol Protocol
) {
    public const string SectionName = "OpenTelemetry";
}