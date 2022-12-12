using Upload.Core;

namespace Upload.Disk;

internal readonly struct DiskFileRef : IFileRef
{
    internal readonly string Path;

    public DiskFileRef(string path, string key, string? url)
    {
        Path = path;
        Key = key;
        Url = url;
    }

    public string Key { get; }
    public string? Url { get; }

    public ValueTask<Stream> OpenRead()
    {
        return ValueTask.FromResult<Stream>(new FileStream(Path, FileMode.Open));
    }

    public ValueTask<bool> Delete()
    {
        try
        {
            File.Delete(Path);
            return ValueTask.FromResult(true);
        }
        catch (Exception)
        {
            // todo: log the exception
            return ValueTask.FromResult(false);
        }
    }

    private static readonly ValueTask<bool> TrueTask = ValueTask.FromResult(true);
    private static readonly ValueTask<bool> FalseTask = ValueTask.FromResult(false);
}