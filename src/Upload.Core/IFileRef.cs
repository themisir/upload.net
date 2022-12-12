namespace Upload.Core;

public interface IFileRef
{
    string Key { get; }
    string? Url { get; }
    ValueTask<Stream> OpenRead();
    ValueTask<bool> Delete();
}