namespace Orders.Integrations.Hub.Core.Infrastructure.Options;

public sealed class PubSubOptions {
    public const string SectionName = "PubSub";
    public required PubSubProvider Provider { get; init; }
    public required TopicsOptions Topics { get; init; }
}

public enum PubSubProvider {
    Sns
}

public sealed record TopicsOptions(
    string AcceptOrder
);