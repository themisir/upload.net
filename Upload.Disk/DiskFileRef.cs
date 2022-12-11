using Upload.Core;

namespace Upload.Disk;

internal readonly struct DiskFileRef : IFileRef
{
    internal readonly string Path;

    public DiskFileRef(string path, string bucket, string key)
    {
        Path = path;
        Bucket = bucket;
        Key = key;
    }

    public string Bucket { get; }
    public string Key { get; }
    
    public Task<Stream> OpenRead()
    {
        return Task.FromResult<Stream>(new FileStream(Path, FileMode.Open));
    }

    public Task<bool> Delete()
    {
        try
        {
            File.Delete(Path);
            return TrueTask;
        }
        catch (Exception)
        {
            // todo: log the exception
            return FalseTask;
        }
    }

    private static readonly Task<bool> TrueTask = Task.FromResult(true);
    private static readonly Task<bool> FalseTask = Task.FromResult(false);
}