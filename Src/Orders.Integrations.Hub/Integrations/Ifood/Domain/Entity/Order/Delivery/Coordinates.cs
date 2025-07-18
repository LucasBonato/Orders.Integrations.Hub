﻿using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Delivery;

public record Coordinates(
    [property: JsonPropertyName("latitude")] decimal Latitude,
    [property: JsonPropertyName("longitude")] decimal Longitude
);