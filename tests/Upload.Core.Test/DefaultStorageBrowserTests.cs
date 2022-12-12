using Upload.Core.Browser;

namespace Upload.Core.Test;

public class DefaultStorageBrowserTests
{
    [Test]
    public void TestGetPublicUrl()
    {
        const string key = "key/to/the/file-01.png";

        var browser = new DefaultStorageBrowser("https://example.com/prefix/objects/{key}");
        var url = browser.GetFileUrl(key);

        url.Should().Be($"https://example.com/prefix/objects/{key}");
    }
}