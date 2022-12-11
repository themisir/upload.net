namespace Upload.Core;

public interface IStorageBrowser
{
    ValueTask<string> GetPublicUrl(IFileRef fileRef);
}