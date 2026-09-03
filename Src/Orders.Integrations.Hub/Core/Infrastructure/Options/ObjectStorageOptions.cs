namespace Orders.Integrations.Hub.Core.Infrastructure.Options;

public sealed class ObjectStorageOptions {
    public const string SectionName = "ObjectStorage";
    public ObjectStorageProvider Provider { get; init; }
    public required BucketOptions Bucket { get; init; }
}

public enum ObjectStorageProvider {
    S3
}

public sealed record BucketOptions( 
    string Name
);