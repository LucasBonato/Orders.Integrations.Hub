namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderTaxInvoice(
    bool Issued,
    string TaxInvoiceURL
);