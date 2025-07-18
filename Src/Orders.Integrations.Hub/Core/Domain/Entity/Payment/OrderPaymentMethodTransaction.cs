﻿using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Payment;

public record OrderPaymentMethodTransaction(
    [property: JsonPropertyName("authorizationCode")] string AuthorizationCode,
    [property: JsonPropertyName("acquirerDocument")] string AcquirerDocument
);