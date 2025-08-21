namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;

public record CancellationMetadata(
    List<string>? Skus
);