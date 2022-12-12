using Upload.Core.Browser;
using Upload.Core.Service;

namespace Upload.Disk;

public sealed class DiskProviderOptions
{
    /// <summary>
    /// Gets or sets absolute path to a file system directory that will be used to store uploaded files.
    /// </summary>
    public string Directory { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets storage browser implementation that'll be used for creating object URLs.
    /// </summary>
    /// <seealso cref="DefaultStorageBrowser"/>
    public IStorageBrowser? Browser { get; set; }
}