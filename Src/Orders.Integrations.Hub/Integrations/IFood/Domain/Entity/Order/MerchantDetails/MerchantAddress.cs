namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.MerchantDetails;

public record MerchantAddress(
    string Country,
    string State,
    string City,
    string PostalCode,
    string District,
    string Street,
    string Number,
    decimal Latitude,
    decimal Longitude
);