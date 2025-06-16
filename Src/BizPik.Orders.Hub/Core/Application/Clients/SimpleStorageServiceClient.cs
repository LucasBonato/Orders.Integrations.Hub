using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using BizPik.Orders.Hub.Core.Domain.Contracts;

namespace BizPik.Orders.Hub.Core.Application.Clients;

public class SimpleStorageServiceClient(
    AmazonS3Client s3Client
) : IObjectStorageClient {
    private readonly string BUCKET_NAME = AppEnv.OBJECT_STORAGE.BUCKET.NAME.NotNull();

    public async Task<string> UploadFile(Stream file, string contentType, string key)
    {
        TransferUtility fileTransferUtility = new(s3Client);

        TransferUtilityUploadRequest request = new() {
            BucketName = BUCKET_NAME,
            InputStream = file,
            ContentType = contentType,
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

    public async Task DeleteFolder(string pathKey)
    {
        ListObjectsV2Request listDeleteRequest = new() {
            BucketName = BUCKET_NAME,
            Prefix = pathKey.EndsWith('/') ? pathKey : pathKey + "/",
        };

        ListObjectsV2Response response;

        do
        {
            response = await s3Client.ListObjectsV2Async(listDeleteRequest);

            if (response.S3Objects.Count > 0)
            {
                DeleteObjectsRequest deleteObjectsRequest = new() {
                    BucketName = BUCKET_NAME,
                    Objects = response.S3Objects.Select(s3Object => new KeyVersion() { Key = s3Object.Key}).ToList()
                };

                await s3Client.DeleteObjectsAsync(deleteObjectsRequest);
            }

            listDeleteRequest.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated);
    }

    private string GetSignedUrl(string key)
    {
        GetPreSignedUrlRequest request = new() {
            BucketName = BUCKET_NAME,
            Key = key,
            Expires = DateTime.Now.AddMinutes(30),
            Verb = HttpVerb.GET,
        };

        AWSConfigsS3.UseSignatureVersion4 = true;

        return s3Client.GetPreSignedURL(request);
    }
}