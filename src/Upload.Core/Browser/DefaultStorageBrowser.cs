using Upload.Core.Service;

namespace Upload.Core.Browser;

public readonly struct DefaultStorageBrowser : IStorageBrowser
{
    private readonly string _urlFormat;

    public DefaultStorageBrowser(string urlFormat)
    {
        _urlFormat = urlFormat;
    }

    public string GetFileUrl(string fileKey)
    {
        var url = _urlFormat.Replace("{key}", fileKey);
        return url;
    }
}