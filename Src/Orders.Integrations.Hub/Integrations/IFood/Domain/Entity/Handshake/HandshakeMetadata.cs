namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;

public class HandshakeMetadata(
    List<Item>? Items,
    List<GarnishItem>? GarnishItems,
    List<Media>? Evidences,
    List<string>? AcceptCancellationReasons
) {
    public List<Item>? Items { get; init; } = Items;

    public List<GarnishItem>? GarnishItems { get; init; } = GarnishItems;

    public List<Media>? Evidences { get; set; } = Evidences;

    public List<string>? AcceptCancellationReasons { get; init; } = AcceptCancellationReasons;
}