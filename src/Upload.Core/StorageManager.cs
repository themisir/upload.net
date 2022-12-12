using Microsoft.Extensions.Options;
using Upload.Core.Service;

namespace Upload.Core;

public sealed class StorageManager
{
    private readonly IOptions<StorageManagerOptions> _options;

    public StorageManager(IOptions<StorageManagerOptions> options)
    {
        _options = options;
    }

    public Task<IFileRef> CreateFile(string backend, string key, Stream sourceStream, UploadOptions? options = null)
    {
        return GetProvider(backend).CreateFile(key, sourceStream, options);
    }

    public Task<IFileRef?> GetFile(string backend, string key)
    {
        return GetProvider(backend).GetFile(key);
    }

    private IStorageProvider GetProvider(string backend)
    {
        if (_options.Value.Providers.TryGetValue(backend, out var impl))
        {
            return impl;
        }

        throw new ApplicationException($"Storage backend with name '{backend}' is not registered");
    }
}