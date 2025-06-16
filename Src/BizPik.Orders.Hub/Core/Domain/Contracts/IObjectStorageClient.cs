namespace BizPik.Orders.Hub.Core.Domain.Contracts;

public interface IObjectStorageClient {
    Task<string> UploadFile(IFormFile file, string key);
    Task DeleteFile(string key);
    Task DeleteFolder(string pathKey);
}