using Upload.Core;
using Upload.Core.Service;

namespace Upload.Disk;

public static class StorageBuilderExtensions
{
    public static StorageBuilder AddDiskBackend(this StorageBuilder builder, string name, Action<DiskProviderOptions> configure)
    {
        var settings = new DiskProviderOptions();
        configure(settings);
        return builder.AddBackend(name, new DiskProvider(settings));
    }
}