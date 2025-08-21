namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;

public record IFoodDisputeAlternativeMetadata(
    Amount? MaxAmount,
    List<int>? AllowedsAdditionalTimeInMinutes,
    List<string>? AllowedsAdditionalTimeReasons
);