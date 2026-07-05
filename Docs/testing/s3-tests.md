# S3 / Object Storage Tests

The `IObjectStorageClient` interface (implemented by `SimpleStorageServiceClient`) handles dispute evidence storage in AWS S3. Tests use `FakeStorage` for unit tests and `TestContainers.LocalStack` for integration tests.

## Interface Under Test

```csharp
public interface IObjectStorageClient
{
    Task<string> UploadFile(Stream file, string contentType, string key);
    Task DeleteFile(string key);
    Task DeleteFolder(string pathKey);
    string GetTemporaryUrl(string key, TimeSpan? expiry = null);
}
```

## FakeStorage for Unit Tests

`FakeStorage` from TestCommon replaces S3 with an in-memory `ConcurrentDictionary`:

```csharp
[Fact]
public async Task UploadFile_ShouldStoreContent()
{
    var storage = new FakeStorage();
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes("dispute-evidence"));

    string key = await storage.UploadFile(stream, "application/pdf", "disputes/ord-123/receipt.pdf");

    Assert.True(storage.Exists("disputes/ord-123/receipt.pdf"));
    Assert.Equal(1, storage.Count);
}

[Fact]
public async Task DeleteFile_ShouldRemoveContent()
{
    var storage = new FakeStorage();
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes("data"));
    await storage.UploadFile(stream, "text/plain", "temp/file.txt");

    await storage.DeleteFile("temp/file.txt");

    Assert.False(storage.Exists("temp/file.txt"));
}

[Fact]
public async Task DeleteFolder_ShouldRemoveAllWithPrefix()
{
    var storage = new FakeStorage();
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes("data"));
    await storage.UploadFile(stream, "text/plain", "disputes/ord-1/file1.pdf");
    await storage.UploadFile(stream, "text/plain", "disputes/ord-1/file2.pdf");

    await storage.DeleteFolder("disputes/ord-1");

    Assert.False(storage.Exists("disputes/ord-1/file1.pdf"));
    Assert.False(storage.Exists("disputes/ord-1/file2.pdf"));
}

[Fact]
public void GetTemporaryUrl_ShouldReturnFormattedUrl()
{
    var storage = new FakeStorage();
    string url = storage.GetTemporaryUrl("disputes/ord-1/receipt.pdf");
    Assert.StartsWith("https://fake-storage.local/", url);
}
```

## LocalStack TestContainers for Integration Tests

For tests that must verify real S3 behavior (presigned URLs, folder pagination):

```csharp
[Collection("LocalStack")]
public class S3IntegrationTests : IAsyncLifetime
{
    private IAmazonS3 _s3Client;
    private SimpleStorageServiceClient _sut;

    public async Task InitializeAsync()
    {
        var localStack = new LocalStackBuilder()
            .WithServices(Service.S3)
            .Build();
        await localStack.StartAsync();

        _s3Client = new AmazonS3Client(localStack.GetConnectionString());
        _sut = new SimpleStorageServiceClient(_s3Client);
    }
}
```

## Dispute Evidence Storage Testing

Verify these scenarios:
1. **Upload** — file content stored and retrievable
2. **Delete** — file removed after dispute resolution
3. **Folder deletion** — all evidence removed when dispute is closed
4. **Presigned URLs** — temporary access with correct expiration
5. **Content types** — PDFs/images stored with correct MIME types
