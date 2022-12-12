using System.Collections.Immutable;
using Upload.Core.Service;

namespace Upload.Core;

public sealed class StorageManager
{
    private readonly IReadOnlyDictionary<string, IStorageProvider> _providers;

    public StorageManager(StorageBuilder builder) : this(builder.Providers.ToImmutableDictionary())
    {
    }
    
    public StorageManager(IReadOnlyDictionary<string, IStorageProvider> providers)
    {
        _providers = providers;
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
        if (_providers.TryGetValue(backend, out var impl))
        {
            return impl;
        }

        throw new ApplicationException($"Storage backend with name '{backend}' is not registered");
    }
}