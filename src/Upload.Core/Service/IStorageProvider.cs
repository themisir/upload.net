namespace Upload.Core.Service;

/// <summary>
/// A simple interface to work with various file storage providers.
/// </summary>
public interface IStorageProvider
{
    /// <summary>
    /// Creates a file with given <paramref name="key"/> and content copied from the <paramref name="sourceStream"/>.
    /// </summary>
    /// <param name="key">Unique key representing the file</param>
    /// <param name="sourceStream">A <see cref="Stream"/> instance to read file contents</param>
    /// <param name="options">Custom upload options</param>
    /// <returns>Task that resolves to a <see cref="IFileRef"/> that points to the uploaded file</returns>
    Task<IFileRef> CreateFile(string key, Stream sourceStream, UploadOptions? options = null);
    
    /// <summary>
    /// Tries to retrieve file from the provider using given <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Unique key representing the file</param>
    /// <returns>Task that resolves to <see cref="IFileRef"/> if the file is being found, or <c>null</c> otherwise.</returns>
    Task<IFileRef?> GetFile(string key);
}