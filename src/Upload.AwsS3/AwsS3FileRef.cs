using Upload.Core;

namespace Upload.AwsS3;

internal sealed class AwsS3FileRef : IFileRef
{
    public AwsS3FileRef(AwsS3Provider owner, string bucket, string key, string? url, Stream? stream)
    {
        Key = key;
        Url = url;
        _bucket = bucket;
        _stream = stream;
        _owner = owner;
    }
    
    private readonly AwsS3Provider _owner;
    private readonly string _bucket;
    private readonly Stream? _stream;
    
    public string Key { get; }
    public string? Url { get; }
    
    public ValueTask<Stream> OpenReadStream()
    {
        return _stream != null ? ValueTask.FromResult(_stream) : OpenLazy();
        
        async ValueTask<Stream> OpenLazy()
        {
            var objectResponse = await _owner.S3Client.GetObjectAsync(_bucket, Key);
            return objectResponse.ResponseStream;
        }
    }

    public async ValueTask<bool> Delete()
    {
        await _owner.S3Client.DeleteObjectAsync(_bucket, Key);
        return true;
    }
}