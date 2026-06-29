using System.Collections.Concurrent;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.TestCommon.Utilities;

public sealed class FakeStorage : IObjectStorageClient
{
    private readonly ConcurrentDictionary<string, (byte[] Content, string ContentType)> _store = new();
    private int _fileCounter;

    public Task<string> UploadFile(Stream file, string contentType, string key) {
        using MemoryStream ms = new();
        file.CopyTo(ms);
        _store[key] = (ms.ToArray(), contentType);
        return Task.FromResult(key);
    }

    public Task DeleteFile(string key) {
        _store.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task DeleteFolder(string pathKey) {
        List<string> keys = _store.Keys.Where(k => k.StartsWith(pathKey)).ToList();
        foreach (string key in keys)
            _store.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public string GetTemporaryUrl(string key, TimeSpan? expiry = null) {
        Interlocked.Increment(ref _fileCounter);
        return $"https://fake-storage.local/{key}?token={_fileCounter}";
    }
}