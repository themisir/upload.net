namespace Upload.Core;

public interface IFileRef
{
    string Bucket { get; }
    string Key { get; }
    Task<Stream> OpenRead();
    Task<bool> Delete();
}