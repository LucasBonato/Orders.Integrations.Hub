namespace Orders.Integrations.Hub.Core.Application.DTOs.Request;

public record CancellationMetadata(
    List<string>? Skus
);