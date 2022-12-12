using Microsoft.Extensions.DependencyInjection;
using Upload.Core.Browser;
using Upload.Core.Service;

namespace Upload.Core.Test;

public class DefaultStorageBrowserTests
{
    [Test]
    public void TestGetPublicUrl()
    {
        const string key = "key/to/the/file-01.png";

        var browser = CreateBrowser("https://example.com/prefix/objects/{key}");
        var url = browser.GetFileUrl(key);

        url.Should().Be($"https://example.com/prefix/objects/{key}");
    }
    
    private static IStorageBrowser CreateBrowser(string format)
    {
        return new ServiceCollection()
            .Configure<DefaultStorageBrowserOptions>(options =>
            {
                options.UrlFormat = format;
            })
            .AddSingleton<IStorageBrowser, DefaultStorageBrowser>()
            .BuildServiceProvider()
            .GetRequiredService<IStorageBrowser>();
    }
}