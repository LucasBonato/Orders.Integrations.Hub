namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderDeliveryInformation(
    string City,
    string CompleteAddress,
    string StreetShortcut,
    string FederalUnit,
    string StreetNumber,
    string Neighborhood,
    string Complement,
    string PostalCode,
    string StreetName
);