using Amazon.S3;
using Amazon.S3.Transfer;
using Upload.Core;
using Upload.Core.Service;

namespace Upload.AwsS3;

public sealed class AwsS3Provider : IStorageProvider
{
    internal readonly IAmazonS3 S3Client;
    internal readonly AwsS3ProviderOptions Options;

    public AwsS3Provider(IAmazonS3 s3Client, AwsS3ProviderOptions options)
    {
        S3Client = s3Client;
        Options = options;
    }

    public async Task<IFileRef> CreateFile(string key, Stream sourceStream, UploadOptions? options = null)
    {
        var uploadOptions = options as AwsS3UploadOptions ?? AwsS3UploadOptions.Default;
        var uploadRequest = uploadOptions.CreateUploadRequest(Options.BucketName, key, sourceStream);
        var fileTransferUtility = new TransferUtility(S3Client);
        var fileUrl = ToUrl(uploadRequest.BucketName, uploadRequest.Key);
        
        await fileTransferUtility.UploadAsync(uploadRequest);

        return new AwsS3FileRef(this, uploadRequest.BucketName, uploadRequest.Key, fileUrl, stream: null);
    }

    public async Task<IFileRef?> GetFile(string key)
    {
        var objectResponse = await S3Client.GetObjectAsync(Options.BucketName, key);
        var fileUrl = ToUrl(objectResponse.BucketName, objectResponse.Key);
        
        return new AwsS3FileRef(this, objectResponse.BucketName, objectResponse.Key, fileUrl, objectResponse.ResponseStream);
    }

    private string? ToUrl(string bucketName,string key)
    {
        if (Options.Browser is { } browser)
        {
            return browser.GetFileUrl(Options.BrowseWithBucket ? Path.Combine(bucketName, key) : key);
        }

        return null;
    }
}