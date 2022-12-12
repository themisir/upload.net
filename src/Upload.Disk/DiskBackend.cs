using Microsoft.Extensions.Options;
using Upload.Core;
using Upload.Core.Extra;

namespace Upload.Disk;

public sealed class DiskBackend : IStorageBackend
{
    private readonly IOptions<DiskBackendSettings> _settings;

    public DiskBackend(IOptions<DiskBackendSettings> settings)
    {
        _settings = settings;
    }

    public async Task<IFileRef> CreateFile(string bucket, string key, Stream sourceStream, UploadOptions? options = null)
    {
        var fileRef = CreateRef(bucket, key);

        // create output directory unless already exists
        Directory.CreateDirectory(Path.GetDirectoryName(fileRef.Path)!);

        await using var fs = TryGuessSize(sourceStream, out var bufferSize)
            ? File.Create(fileRef.Path, bufferSize)
            : File.Create(fileRef.Path);
        
        await sourceStream.CopyToAsync(fs);

        return fileRef;
    }

    public Task<IFileRef?> GetFile(string bucket, string key)
    {
        var fileRef = CreateRef(bucket, key);
        return Task.FromResult<IFileRef?>(
            File.Exists(fileRef.Path) 
                ? fileRef 
                : null);
    }

    private DiskFileRef CreateRef(string bucket, string key)
    {
        KeyUtils.MushBeSafeKey(key);
        var normalizedKey = KeyUtils.NormalizeKey(key);
        var path = Path.Combine(_settings.Value.Directory, bucket, normalizedKey);
        return new DiskFileRef(path, bucket, normalizedKey);
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