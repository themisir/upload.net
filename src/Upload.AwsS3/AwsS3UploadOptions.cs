using Amazon.S3;
using Amazon.S3.Transfer;
using Upload.Core;

namespace Upload.AwsS3;

public sealed class AwsS3UploadOptions : UploadOptions
{
    public new static readonly AwsS3UploadOptions Default = new();
    
    public S3CannedACL? CannedAcl { get; set; }
    public S3StorageClass? S3StorageClass { get; set; }
    public Func<TransferUtilityUploadRequest>? UploadRequestBuilder { get; set; }

    internal TransferUtilityUploadRequest CreateUploadRequest(string bucketName, string key, Stream stream)
    {
        var request = UploadRequestBuilder is { } builder
            ? builder()
            : new TransferUtilityUploadRequest
            {
                CannedACL = CannedAcl,
                StorageClass = S3StorageClass,
                // R2 doesn't work without this 
                DisablePayloadSigning = true
            };

        request.Key = key;
        request.BucketName = bucketName;
        request.InputStream = stream;

        return request;
    }
}