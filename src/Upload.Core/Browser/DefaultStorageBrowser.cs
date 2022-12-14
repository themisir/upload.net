using Upload.Core.Service;

namespace Upload.Core.Browser;

public readonly struct DefaultStorageBrowser : IStorageBrowser
{
    private readonly string _urlFormat;

    /// <summary>
    /// A simplest <see cref="IStorageBrowser"/> that uses url template to generate file URLs. The implementation works
    /// by replacing the {key} template parameter of the URL template with the given file key.
    /// </summary>
    /// <param name="urlFormat">URL template</param>
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