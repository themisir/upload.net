using Upload.Core.Service;

namespace Upload.Core;

/// <summary>
/// A interface containing useful methods to work with uploaded files on a specific provider. Usually created and
/// managed by specific <see cref="IStorageProvider"/> implementations.
/// </summary>
public interface IFileRef
{
    /// <summary>
    /// Gets unique identifier of the file reference. Usually it represents file name or blob object key.
    /// </summary>
    string Key { get; }
    
    /// <summary>
    /// Gets url to the file if available.
    /// </summary>
    string? Url { get; }
    
    /// <summary>
    /// Creates a stream to read contents of the file.
    /// </summary>
    /// <returns>An instance of <see cref="Stream"/> for reading file contents</returns>
    ValueTask<Stream> OpenReadStream();
    
    /// <summary>
    /// Tries to delete file from the provider.
    /// </summary>
    /// <returns>Task that resolves to a boolean representing whether or not the deletion has been succeeded.</returns>
    ValueTask<bool> Delete();
}