using Microsoft.Extensions.Options;
using Upload.Core.Service;

namespace Upload.Core.Browser;

public sealed class DefaultStorageBrowser : IStorageBrowser
{
    private readonly IOptions<DefaultStorageBrowserSettings> _settings;

    public DefaultStorageBrowser(IOptions<DefaultStorageBrowserSettings> settings)
    {
        _settings = settings;
    }

    public string GetFileUrl(string fileKey)
    {
        var url = _settings.Value.UrlFormat.Replace("{key}", fileKey);
        return url;
    }
}