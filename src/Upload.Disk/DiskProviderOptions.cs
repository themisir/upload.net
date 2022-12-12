using Upload.Core.Service;

namespace Upload.Disk;

public sealed class DiskProviderOptions
{
    public string Directory { get; set; } = null!;
    public IStorageBrowser? Browser { get; set; }
}