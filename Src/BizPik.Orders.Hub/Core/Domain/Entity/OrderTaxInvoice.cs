using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.Entity;

public record OrderTaxInvoice(
    [property: JsonPropertyName("issued")] bool Issued,
    [property: JsonPropertyName("taxInvoiceURL")] string TaxInvoiceURL
);