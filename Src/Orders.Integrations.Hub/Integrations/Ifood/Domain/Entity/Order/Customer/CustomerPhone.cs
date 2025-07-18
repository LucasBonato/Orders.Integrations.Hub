﻿using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Customer;

public record CustomerPhone(
    [property: JsonPropertyName("number")] string Number,
    [property: JsonPropertyName("localizer")] string Localizer,
    [property: JsonPropertyName("localizerExpiration")] DateTime LocalizerExpiration
);