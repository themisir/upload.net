using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Upload.Core.Test;

public class DefaultStorageBrowserTests
{
    [Test]
    public async Task TestGetPublicUrl()
    {
        var mock = new Mock<IFileRef>();

        const string bucket = "bucketName";
        const string key = "key/to/the/file-01.png";

        mock.Setup(file => file.Bucket)
            .Returns(bucket);

        mock.Setup(file => file.Key)
            .Returns(key);

        var browser = CreateBrowser("https://example.com/prefix/{bucket}/objects/{key}");
        var url = await browser.GetPublicUrl(mock.Object);

        url.Should().Be($"https://example.com/prefix/{bucket}/objects/{key}");
    }
    
    private static IStorageBrowser CreateBrowser(string format)
    {
        return new ServiceCollection().AddDefaultStorageBrowser(options =>
            {
                options.UrlFormat = format;
            })
            .BuildServiceProvider()
            .GetRequiredService<IStorageBrowser>();
    }
}