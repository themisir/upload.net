using Microsoft.Extensions.Options;

namespace Upload.Core.Browser;

public sealed class DefaultStorageBrowser : IStorageBrowser
{
    private readonly IOptions<DefaultStorageBrowserSettings> _settings;

    public DefaultStorageBrowser(IOptions<DefaultStorageBrowserSettings> settings)
    {
        _settings = settings;
    }

    public ValueTask<string> GetPublicUrl(IFileRef fileRef)
    {
        var url = _settings.Value.UrlFormat
            .Replace("{bucket}", fileRef.Bucket)
            .Replace("{key}", fileRef.Key);
        
        return ValueTask.FromResult(url);
    }
}