using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;

namespace Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;

public class SimpleStorageServiceClient(
    IAmazonS3 s3Client
) : IObjectStorageClient {
    private readonly string _bucketName = AppEnv.OBJECT_STORAGE.BUCKET.NAME.NotNullEnv();

    public async Task<string> UploadFile(Stream file, string contentType, string key)
    {
        TransferUtility fileTransferUtility = new(s3Client);

        await fileTransferUtility.UploadAsync(new TransferUtilityUploadRequest {
            BucketName = _bucketName,
            InputStream = file,
            ContentType = contentType,
            StorageClass = S3StorageClass.Standard,
            Key = key,
            Headers = {
                CacheControl = "max-age=0, must-revalidate"
            }
        });

        return key;
    }

    public async Task DeleteFile(string key)
    {
        await s3Client.DeleteObjectAsync(_bucketName, key);
    }

    public async Task DeleteFolder(string pathKey)
    {
        ListObjectsV2Request listDeleteRequest = new() {
            BucketName = _bucketName,
            Prefix = pathKey.EndsWith('/') ? pathKey : pathKey + "/",
        };

        ListObjectsV2Response response;

        do {
            response = await s3Client.ListObjectsV2Async(listDeleteRequest);

            if (response.S3Objects.Count > 0)
                await s3Client.DeleteObjectsAsync(new DeleteObjectsRequest {
                    BucketName = _bucketName,
                    Objects = response.S3Objects
                        .Select(s3Object => new KeyVersion { Key = s3Object.Key})
                        .ToList()
                });

            listDeleteRequest.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated?? false);
    }

    public string GetTemporaryUrl(string key, TimeSpan? expiry = null) {
        return s3Client.GetPreSignedURL(new GetPreSignedUrlRequest {
            BucketName = _bucketName, 
            Key = key, 
            Expires = DateTime.UtcNow.Add(expiry?? TimeSpan.FromMinutes(30)), 
            Verb = HttpVerb.GET,
        });
    }
}