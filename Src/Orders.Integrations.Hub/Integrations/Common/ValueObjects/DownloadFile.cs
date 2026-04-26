namespace Orders.Integrations.Hub.Integrations.Common.ValueObjects;

public record DownloadFile(
    byte[] Bytes,
    string ContentType = "application/octet-stream"
);