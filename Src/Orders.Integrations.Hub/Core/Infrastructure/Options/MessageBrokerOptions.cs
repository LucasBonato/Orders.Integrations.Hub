namespace Orders.Integrations.Hub.Core.Infrastructure.Options;

public sealed record MessageBrokerOptions(
    MessageBrokerProvider Provider,
    MessageBrokerRetryOptions Retry,
    MessageBrokerCircuitBreakerOptions CircuitBreaker
) {
    public const string SectionName = "MessageBroker";
};

public enum MessageBrokerProvider {
    Memory,
    RabbitMq
}

public sealed record MessageBrokerRetryOptions(
    int RetryLimit = 5,
    int MinIntervalSeconds = 1,
    int MaxIntervalSeconds = 120,
    int IntervalDeltaSeconds = 5
);

public sealed record MessageBrokerCircuitBreakerOptions(
    int TrackingPeriodMinutes = 1,
    int TripThreshold = 15,
    int ActiveThreshold = 10,
    int ResetIntervalMinutes = 5
);