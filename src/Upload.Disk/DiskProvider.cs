using Upload.Core;
using Upload.Core.Extra;
using Upload.Core.Service;

namespace Upload.Disk;

public sealed class DiskProvider : IStorageProvider
{
    private readonly DiskProviderSettings _settings;
    private readonly IStorageBrowser? _browser;

    public DiskProvider(DiskProviderSettings settings)
    {
        _settings = settings;
        _browser = settings.Browser;
    }

    public async Task<IFileRef> CreateFile(string key, Stream sourceStream, UploadOptions? options = null)
    {
        var fileRef = CreateRef(key);

        // create output directory unless already exists
        Directory.CreateDirectory(Path.GetDirectoryName(fileRef.Path)!);

        await using var fs = TryGuessSize(sourceStream, out var bufferSize)
            ? File.Create(fileRef.Path, bufferSize)
            : File.Create(fileRef.Path);
        
        await sourceStream.CopyToAsync(fs);

        return fileRef;
    }

    public Task<IFileRef?> GetFile(string key)
    {
        var fileRef = CreateRef(key);
        return Task.FromResult<IFileRef?>(
            File.Exists(fileRef.Path) 
                ? fileRef 
                : null);
    }

    private DiskFileRef CreateRef(string key)
    {
        KeyUtils.MushBeSafeKey(key);
        var normalizedKey = KeyUtils.NormalizeKey(key);
        var path = Path.Combine(_settings.Directory, normalizedKey);
        var url = _browser?.GetFileUrl(normalizedKey);
        return new DiskFileRef(path, normalizedKey, url);
    }

    private static bool TryGuessSize(Stream stream, out int size)
    {
        if (stream is MemoryStream memoryStream)
        {
            size = (int)memoryStream.Length;
            return true;
        }

        size = default;
        return false;
    }
}