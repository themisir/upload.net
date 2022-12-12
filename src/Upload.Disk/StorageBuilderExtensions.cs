using Upload.Core;

namespace Upload.Disk;

public static class StorageBuilderExtensions
{
    public static StorageBuilder AddDiskBackend(this StorageBuilder builder, string name, Action<DiskProviderSettings> configure)
    {
        var settings = new DiskProviderSettings();
        configure(settings);
        return builder.AddBackend(name, new DiskProvider(settings));
    }
}