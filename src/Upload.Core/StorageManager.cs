using System.Diagnostics.CodeAnalysis;
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

    public Task<IFileRef> CreateFile(string providerName, string key, Stream sourceStream, UploadOptions? options = null)
    {
        return GetProvider(providerName).CreateFile(key, sourceStream, options);
    }

    public Task<IFileRef?> GetFile(string providerName, string key)
    {
        return GetProvider(providerName).GetFile(key);
    }

    public bool TryGetProvider(string providerName, [MaybeNullWhen(false)] out IStorageProvider provider)
    {
        return _options.Value.Providers.TryGetValue(providerName, out provider);
    }

    private IStorageProvider GetProvider(string providerName)
    {
        if (TryGetProvider(providerName, out var impl))
        {
            return impl;
        }

        throw new ApplicationException($"Storage provider with name '{providerName}' is not registered");
    }
}