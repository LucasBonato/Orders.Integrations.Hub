namespace Orders.Integrations.Hub.Integrations.Common;

public record DownloadFile(
    byte[] Bytes,
    string ContentType = "application/octet-stream"
);