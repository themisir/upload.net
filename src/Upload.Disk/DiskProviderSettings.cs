using Upload.Core.Service;

namespace Upload.Disk;

public sealed class DiskProviderSettings
{
    public string Directory { get; set; } = null!;
    public IStorageBrowser? Browser { get; set; }
}