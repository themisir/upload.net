using Microsoft.Extensions.DependencyInjection;
using Upload.Core;

namespace Upload.Disk.Test;

public sealed class DiskBackendTests
{
    private string _tempDir = null!;
    private IStorageBackend _backend = null!;

    [SetUp]
    public void SetUp()
    {
        _tempDir = Directory.CreateTempSubdirectory().FullName;
        _backend = CreateBackend(_tempDir);
    }

    [TearDown]
    public void TearDown()
    {
        Directory.Delete(_tempDir, true);
    }

    [Test]
    public async Task TestCreateFile()
    {
        var uploadContents = "sample content"u8.ToArray();

        using var ms = new MemoryStream(uploadContents);

        var file = await _backend.CreateFile("myBucket", "01\\test.txt", ms, UploadOptions.Default);
        file.Bucket.Should().Be("myBucket");
        file.Key.Should().Be("01/test.txt");

        var actualContents = await File.ReadAllBytesAsync(Path.Join(_tempDir, "myBucket/01/test.txt"));
        actualContents.Should().BeEquivalentTo(uploadContents);
    }

    [Test]
    public async Task TestGetFile()
    {
        var uploadContents = "sample content"u8.ToArray();

        using var ms = new MemoryStream(uploadContents);

        await _backend.CreateFile("myBucket2", "02/test.bin", ms, UploadOptions.Default);
        
        var file = await _backend.GetFile("myBucket2", "02/test.bin");
        
        file.Should().NotBeNull();
        file!.Bucket.Should().Be("myBucket2");
        file.Key.Should().Be("02/test.bin");

        await using var stream = await file.OpenRead();
        stream.Should().BeReadable();

        var buff = new byte[1024];
        var read = stream.Read(buff);
        var readContents = buff[..read];

        readContents.Should().BeEquivalentTo(uploadContents);
    }
    
    private static IStorageBackend CreateBackend(string dir)
    {
        return new ServiceCollection().AddDiskBackend(options =>
            {
                options.Directory = dir;
            })
            .BuildServiceProvider()
            .GetRequiredService<IStorageBackend>();
    }
}