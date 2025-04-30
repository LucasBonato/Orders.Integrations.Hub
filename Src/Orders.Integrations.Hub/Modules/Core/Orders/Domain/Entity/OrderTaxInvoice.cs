using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity;

public record OrderTaxInvoice(
    [property: JsonPropertyName("issued")] bool Issued,
    [property: JsonPropertyName("taxInvoiceURL")] string TaxInvoiceURL
);