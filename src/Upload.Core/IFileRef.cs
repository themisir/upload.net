namespace Upload.Core;

public interface IFileRef
{
    string Key { get; }
    string? Url { get; }
    ValueTask<Stream> OpenReadStream();
    ValueTask<bool> Delete();
}