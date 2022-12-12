using Microsoft.Extensions.Options;
using Upload.Core.Service;

namespace Upload.Core.Browser;

public sealed class DefaultStorageBrowser : IStorageBrowser
{
    private readonly IOptions<DefaultStorageBrowserOptions> _options;

    public DefaultStorageBrowser(IOptions<DefaultStorageBrowserOptions> options)
    {
        _options = options;
    }

    public string GetFileUrl(string fileKey)
    {
        var url = _options.Value.UrlFormat.Replace("{key}", fileKey);
        return url;
    }
}