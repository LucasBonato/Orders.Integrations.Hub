using NBomber.Data;

using Orders.Integrations.Hub.IntegrationTests.Payloads;

namespace Orders.Integrations.Hub.LoadTests.Payloads;

public sealed record WebhookPayload(string EventType, string RawJson);

/// <summary>
/// Wraps PayloadLoader-sourced fixtures in NBomber.Data feeds so each webhook
/// scenario cycles through realistic, varied event types rather than only the
/// happy path. Template names match the actual files in TestCommon/Payloads/Templates/.
/// </summary>
public static class WebhookPayloadFeeds
{
    public static IDataFeed<WebhookPayload> IFood() => DataFeed.Random(
    [
        new WebhookPayload("KEEPALIVE", PayloadLoader.Load("IFood", "keepalive")),
        new WebhookPayload("ORDER_CONFIRMED", PayloadLoader.Load("IFood", "confirmed")),
        new WebhookPayload("ORDER_CANCELLED", PayloadLoader.Load("IFood", "cancelled")),
    ]);

    public static IDataFeed<WebhookPayload> Food99() => DataFeed.Random(
    [
        new WebhookPayload("order.new", PayloadLoader.Load("Food99", "order-new")),
        new WebhookPayload("order.update", PayloadLoader.Load("Food99", "order-update")),
    ]);

    public static IDataFeed<WebhookPayload> RappiCreate() => DataFeed.Random(
    [
        new WebhookPayload("ORDER_CREATED", PayloadLoader.Load("Rappi", "create")),
    ]);

    public static IDataFeed<WebhookPayload> RappiCancel() => DataFeed.Random(
    [
        new WebhookPayload("ORDER_CANCELLED", PayloadLoader.Load("Rappi", "cancel")),
    ]);

    public static IDataFeed<WebhookPayload> RappiOther() => DataFeed.Random(
    [
        new WebhookPayload("ORDER_OTHER", PayloadLoader.Load("Rappi", "other")),
    ]);

    public static IDataFeed<WebhookPayload> RappiPing() => DataFeed.Random(
    [
        new WebhookPayload("PING", PayloadLoader.Load("Rappi", "ping")),
    ]);
}
