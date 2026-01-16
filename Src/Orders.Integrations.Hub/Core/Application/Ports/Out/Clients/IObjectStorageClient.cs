namespace Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

public interface IObjectStorageClient {
    Task<string> UploadFile(Stream file, string contentType, string key);
    Task DeleteFile(string key);
    Task DeleteFolder(string pathKey);
}