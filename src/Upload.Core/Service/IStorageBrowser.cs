namespace Upload.Core.Service;

public interface IStorageBrowser
{
    /// <summary>
    /// Converts given file key into a URL.
    /// </summary>
    /// <param name="fileKey">The file key</param>
    /// <returns>URL string</returns>
    string GetFileUrl(string fileKey);
}