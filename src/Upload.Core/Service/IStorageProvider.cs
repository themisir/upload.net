namespace Upload.Core.Service;

public interface IStorageProvider
{
    Task<IFileRef> CreateFile(string key, Stream sourceStream, UploadOptions? options = null);
    Task<IFileRef?> GetFile(string key);
}