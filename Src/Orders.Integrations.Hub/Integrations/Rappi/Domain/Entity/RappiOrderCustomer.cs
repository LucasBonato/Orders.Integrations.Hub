namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderCustomer(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string DocumentType,
    string DocumentNumber
);