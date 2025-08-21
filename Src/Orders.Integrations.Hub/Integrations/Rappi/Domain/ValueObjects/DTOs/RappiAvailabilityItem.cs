namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiAvailabilityItem(
    List<string> TurnOn,
    List<string> TurnOff
);