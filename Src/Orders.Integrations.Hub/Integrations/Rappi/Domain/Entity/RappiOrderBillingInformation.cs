namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderBillingInformation(
    string BillingType,
    string Name,
    string? Address,
    string? Phone,
    string? Email,
    string DocumentType,
    string DocumentNumber
);