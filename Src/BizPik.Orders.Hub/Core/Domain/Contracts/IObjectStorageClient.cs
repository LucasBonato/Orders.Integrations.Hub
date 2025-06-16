namespace BizPik.Orders.Hub.Core.Domain.Contracts;

public interface IObjectStorageClient {
    Task<string> UploadFile(Stream file, string contentType, string key);
    Task DeleteFile(string key);
    Task DeleteFolder(string pathKey);
}