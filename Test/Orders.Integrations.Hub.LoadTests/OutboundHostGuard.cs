using Microsoft.Extensions.Configuration;

namespace Orders.Integrations.Hub.LoadTests.Infrastructure;

/// <summary>
/// Refuses to let the load test run if any integration base URL resolves outside
/// the local WireMock container. This is independent of, and in addition to, the
/// config-mapping logic itself — the point is to catch the case where the mapping
/// was skipped, mistyped, or a new config key was added upstream without the
/// override being wired here.
/// </summary>
public static class OutboundHostGuard
{
    // Every config key that can point at a partner/internal HTTP endpoint.
    // Keep this in sync with Docs/README "Configuration" section and the
    // Outbound Boundaries listed in the load-test discovery doc.
    private static readonly string[] IntegrationUrlKeys =
    [
        "Clients:Orders:BaseUrl",
        "Clients:Internal:BaseUrl",
        "Integrations:IFood:Endpoint:BaseUrl",
        "Integrations:IFood:Endpoint:AuthUrl",
        "Integrations:Rappi:Endpoint:BaseUrl",
        "Integrations:Rappi:Endpoint:AuthUrl",
        "Integrations:Food99:Endpoint:BaseUrl",
    ];

    public static void Validate(IConfiguration configuration)
    {
        List<string> offenders = [];

        foreach (string key in IntegrationUrlKeys)
        {
            string? value = configuration[key];
            if (value is null)
                continue;

            if (!IsLocalOrContainerHost(value))
                offenders.Add($"{key} = {value}");
        }

        if (offenders.Count > 0)
        {
            throw new InvalidOperationException(
                "Refusing to start the load test: the following config keys resolve " +
                "outside localhost / the WireMock container, which would send real " +
                $"traffic to a partner API:{Environment.NewLine}" +
                string.Join(Environment.NewLine, offenders));
        }
    }

    private static bool IsLocalOrContainerHost(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? parsed))
            return false;

        return parsed.Host is "localhost" or "127.0.0.1" or "::1"
               // Testcontainers on Linux typically exposes via 127.0.0.1 with a
               // mapped port; on some CI runners the container host can differ
               // (e.g. host.docker.internal). Add any such host explicitly here
               // rather than loosening this check with a wildcard.
               || parsed.Host == "host.docker.internal";
    }
}