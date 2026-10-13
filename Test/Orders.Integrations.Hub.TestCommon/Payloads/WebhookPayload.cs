using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.IntegrationTests.Payloads;

namespace Orders.Integrations.Hub.TestCommon.Payloads;

public sealed record WebhookPayload(
    string EventType, 
    string Payload,
    string? Endpoint = ""
);

public static class WebhookPayloadFeeds {
    public static IEnumerable<WebhookPayload> IFoodEvents() => [
        new(nameof(IFoodFullOrderStatus.KEEPALIVE),         PayloadLoader.Load("IFood", "event-order-keepalive")),
        new(nameof(IFoodFullOrderStatus.PLACED),            PayloadLoader.Load("IFood", "event-order-placed")),
        new(nameof(IFoodFullOrderStatus.CONFIRMED),         PayloadLoader.Load("IFood", "event-order-confirmed")),
        new(nameof(IFoodFullOrderStatus.CANCELLED),         PayloadLoader.Load("IFood", "event-order-cancelled")),
        new(nameof(IFoodFullOrderStatus.HANDSHAKE_DISPUTE), PayloadLoader.Load("IFood", "event-dispute-handshake")),
        new("UNKNOWN",                                      PayloadLoader.Load("IFood", "unknown"))
    ];

    // public static IEnumerable<WebhookPayload> RappiEvents() => [
    //     new(,         PayloadLoader.Load("Rappi", "event-order-created")),
    //     new(OrderEventType.CONFIRMED,       PayloadLoader.Load("Rappi", "event-order-accepted")),
    //     new(OrderEventType.CONFIRMED,       PayloadLoader.Load("Rappi", "event-order-accepted"))
    // ];

    public static IEnumerable<WebhookPayload> Food99Events() => [
        new(nameof(Food99Type.OrderNew),           PayloadLoader.Load("Food99", "event-order-confirmed")),
        new(nameof(Food99Type.OrderCancel),        PayloadLoader.Load("Food99", "event-order-cancelled")),
        new(nameof(Food99Type.DeliveryStatus),     PayloadLoader.Load("Food99", "event-order-cancelled")),
        new(nameof(Food99Type.OrderPartialCancel), PayloadLoader.Load("Food99", "event-order-cancelled")),
        new(nameof(Food99Type.OrderFinish),        PayloadLoader.Load("Food99", "event-order-cancelled")),
        new(nameof(Food99Type.OrderCancelApply),   PayloadLoader.Load("Food99", "event-cancel-apply")),
        new(nameof(Food99Type.OrderRefundApply),   PayloadLoader.Load("Food99", "event-refund-apply")),
    ];
}