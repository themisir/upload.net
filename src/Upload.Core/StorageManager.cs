using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Upload.Core.Service;

namespace Upload.Core;

/// <summary>
/// An utility class to work with various <see cref="IStorageProvider"/> instances at once.
/// </summary>
public sealed class StorageManager
{
    private readonly IOptions<StorageManagerOptions> _options;

    public StorageManager(IOptions<StorageManagerOptions> options)
    {
        _options = options;
    }

    /// <summary>
    /// Creates a file with given <paramref name="key"/> and content copied from the <paramref name="sourceStream"/> on
    /// the specific provider.
    /// </summary>
    /// <param name="providerName">Name of the provider specified when registering it with <see cref="StorageBuilder.AddProvider"/></param>
    /// <param name="key">Unique key representing the file</param>
    /// <param name="sourceStream">A <see cref="Stream"/> instance to read file contents</param>
    /// <param name="options">Custom upload options</param>
    /// <returns>Task that resolves to a <see cref="IFileRef"/> that points to the uploaded file</returns>
    public Task<IFileRef> CreateFile(string providerName, string key, Stream sourceStream, UploadOptions? options = null)
    {
        return GetProvider(providerName).CreateFile(key, sourceStream, options);
    }

    /// <summary>
    /// Tries to retrieve file from the provider using given <paramref name="key"/> from a specific provider.
    /// </summary>
    /// <param name="providerName">Name of the provider specified when registering it with <see cref="StorageBuilder.AddProvider"/></param>
    /// <param name="key">Unique key representing the file</param>
    /// <returns>Task that resolves to <see cref="IFileRef"/> if the file is being found, or <c>null</c> otherwise.</returns>
    public Task<IFileRef?> GetFile(string providerName, string key)
    {
        return GetProvider(providerName).GetFile(key);
    }

    #region GetProvider

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
    
    #endregion
}