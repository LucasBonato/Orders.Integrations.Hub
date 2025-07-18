﻿using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Total(
    [property: JsonPropertyName("additionalFees")] decimal AdditionalFees,
    [property: JsonPropertyName("subTotal")] decimal SubTotal,
    [property: JsonPropertyName("deliveryFee")] decimal DeliveryFee,
    [property: JsonPropertyName("benefits")] decimal Benefits,
    [property: JsonPropertyName("orderAmount")] decimal OrderAmount
);