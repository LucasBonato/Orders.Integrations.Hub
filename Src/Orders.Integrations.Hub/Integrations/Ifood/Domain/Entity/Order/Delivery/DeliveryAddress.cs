namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Delivery;

public record DeliveryAddress(
    string StreetName,
    string StreetNumber,
    string FormattedAddress,
    string Neighborhood,
    string Complement,
    string Reference,
    string PostalCode,
    string City,
    string State,
    string Country,
    Coordinates Coordinates
);