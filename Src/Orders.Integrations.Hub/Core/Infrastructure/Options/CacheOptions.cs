namespace Orders.Integrations.Hub.Core.Infrastructure.Options;

public sealed record CacheOptions(
    CacheProvider Provider
) {
    public const string SectionName = "Cache";
}

public enum CacheProvider {
    Memory,
    Distributed,
    Hybrid
}