using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Upload.Core;

namespace Upload.Disk.Test;

public class DiskBackendTest
{
    private static IStorageBackend CreateBackend(string dir)
    {
        return new ServiceCollection().AddDiskBackend(options =>
            {
                options.Directory = dir;
            })
            .BuildServiceProvider()
            .GetRequiredService<IStorageBackend>();
    }

    [Fact]
    public async Task CreateAndGetFileTest()
    {
        var uploadContents = "sample content"u8.ToArray();
        var dir = Directory.CreateTempSubdirectory().FullName;
        
        try
        {
            var backend = CreateBackend(dir);

            using var ms = new MemoryStream(uploadContents);

            var file = await backend.CreateFile("myBucket", "01\\test.txt", ms, UploadOptions.Default);

            file.Bucket.Should().Be("myBucket");
            file.Key.Should().Be("01/test.txt");

            var actualContents = await File.ReadAllBytesAsync(Path.Join(dir, "myBucket/01/test.txt"));

            actualContents.Should().BeEquivalentTo(uploadContents);

            var file2 = await backend.GetFile("myBucket", "01/test.txt");

            file2.Should().NotBeNull();
            file2!.Bucket.Should().Be("myBucket");
            file2.Key.Should().Be("01/test.txt");

            await using var stream = await file2.OpenRead();
            stream.Should().BeReadable();

            var buff = new byte[1024];
            var read = stream.Read(buff);
            var readContents = buff[..read];

            readContents.Should().BeEquivalentTo(uploadContents);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }
}