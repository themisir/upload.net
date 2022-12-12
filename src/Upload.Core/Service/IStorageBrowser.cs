namespace Upload.Core.Service;

public interface IStorageBrowser
{
    string GetFileUrl(string fileKey);
}