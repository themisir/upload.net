using Upload.Core;
using Upload.Core.Service;

namespace Upload.Disk.Test;

public sealed class DiskBackendTests
{
    private string _tempDir = null!;
    private IStorageProvider _provider = null!;

    [SetUp]
    public void SetUp()
    {
        _tempDir = Directory.CreateTempSubdirectory().FullName;
        _provider = CreateBackend(_tempDir);
    }

    [TearDown]
    public void TearDown()
    {
        Directory.Delete(_tempDir, true);
    }

    [Test]
    public async Task TestCreateFile()
    {
        using var ms = new MemoryStream(Array.Empty<byte>());
        var file = await _provider.CreateFile("01\\test.txt", ms, UploadOptions.Default);
        var fileExists = File.Exists(Path.Join(_tempDir, "01/test.txt"));
        
        file.Key.Should().Be("01/test.txt");
        fileExists.Should().BeTrue();
    }

    [Test]
    public async Task TestGetFile()
    {
        var uploadContents = "sample content"u8.ToArray();

        using var ms = new MemoryStream(uploadContents);

        await _provider.CreateFile("02/test.bin", ms, UploadOptions.Default);
        
        var file = await _provider.GetFile("02/test.bin");
        file.Should().NotBeNull();
        file!.Key.Should().Be("02/test.bin");

        await using var stream = await file.OpenReadStream();
        stream.Should().BeReadable();

        var buff = new byte[1024];
        var read = stream.Read(buff);
        var readContents = buff[..read];

        readContents.Should().BeEquivalentTo(uploadContents);
    }

    [Test]
    public async Task TestDeleteFile()
    {
        using var ms = new MemoryStream(Array.Empty<byte>());
        await _provider.CreateFile("03/test", ms, UploadOptions.Default);
        
        var file = await _provider.GetFile("03/test");
        var deleted = await file!.Delete();
        var fileExists = File.Exists(Path.Join(_tempDir, "03/test"));

        deleted.Should().BeTrue();
        fileExists.Should().BeFalse();
    }
    
    private static IStorageProvider CreateBackend(string dir)
    {
        return new DiskProvider(new DiskProviderOptions
        {
            Directory = dir,
        });
    }
}