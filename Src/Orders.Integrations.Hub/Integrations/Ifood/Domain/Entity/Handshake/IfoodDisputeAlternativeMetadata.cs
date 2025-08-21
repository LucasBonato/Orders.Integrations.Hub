namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record IfoodDisputeAlternativeMetadata(
    Amount? MaxAmount,
    List<int>? AllowedsAdditionalTimeInMinutes,
    List<string>? AllowedsAdditionalTimeReasons
);