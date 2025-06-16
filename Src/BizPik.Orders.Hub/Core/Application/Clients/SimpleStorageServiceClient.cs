using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using BizPik.Orders.Hub.Core.Domain.Contracts;

namespace BizPik.Orders.Hub.Core.Application.Clients;

public class SimpleStorageServiceClient(
    AmazonS3Client s3Client
) : IObjectStorageClient {
    private readonly string BUCKET_NAME = AppEnv.OBJECT_STORAGE.BUCKET.NAME.NotNull();

    public async Task<string> UploadFile(IFormFile file, string key)
    {
        TransferUtility fileTransferUtility = new(s3Client);

        Stream streamFile = file.OpenReadStream();

        TransferUtilityUploadRequest request = new() {
            BucketName = BUCKET_NAME,
            InputStream = streamFile,
            ContentType = file.ContentType,
            StorageClass = S3StorageClass.Standard,
            Key = key,
            Headers = {
                CacheControl = "max-age=0, must-revalidate"
            }
        };

        await fileTransferUtility.UploadAsync(request);

        return GetSignedUrl(request.Key);
    }

    public async Task DeleteFile(string key)
    {
        await s3Client.DeleteObjectAsync(BUCKET_NAME, key);
    }

    private string GetSignedUrl(string key)
    {
        GetPreSignedUrlRequest request = new() {
            BucketName = BUCKET_NAME,
            Key = key,
            Expires = DateTime.Now.AddMinutes(30),
            Verb = HttpVerb.GET
        };

        return s3Client.GetPreSignedURL(request);
    }
}