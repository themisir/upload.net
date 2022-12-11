namespace Upload.Core;

public interface IStorageBackend
{
    Task<IFileRef> CreateFile(string bucket, string key, UploadOptions? options = null);
    Task<IFileRef> GetFile(string bucket, string key);
}