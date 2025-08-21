namespace Orders.Integrations.Hub.Core.Domain.Entity.Address;

public record Address(
    string Country,
    string State,
    string City,
    string District,
    string Street,
    string Number,
    string Complement,
    string Reference,
    string FormattedAddress,
    string PostalCode,
    AddressCoordinates Coordinates
);