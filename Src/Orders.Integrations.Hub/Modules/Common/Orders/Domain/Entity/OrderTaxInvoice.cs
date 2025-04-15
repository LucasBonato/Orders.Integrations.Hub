using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Adapter.Dtos.Entity;

public record OrderTaxInvoice(
    [property: JsonPropertyName("issued")] bool Issued,
    [property: JsonPropertyName("taxInvoiceURL")] string TaxInvoiceURL
);