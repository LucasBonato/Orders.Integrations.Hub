﻿using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderTaxInvoice(
    [property: JsonPropertyName("issued")] bool Issued,
    [property: JsonPropertyName("taxInvoiceURL")] string TaxInvoiceURL
);