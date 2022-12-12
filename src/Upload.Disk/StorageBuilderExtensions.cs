using Upload.Core.Service;

namespace Upload.Disk;

public static class StorageBuilderExtensions
{
    /// <summary>
    /// Registers a Disk storage backend provider with given <paramref name="name"/>.
    /// </summary>
    /// <param name="builder">Storage configuration builder</param>
    /// <param name="name">Unique provider name</param>
    /// <param name="configure">Configuration delegate</param>
    /// <returns>Reference to <paramref name="builder"/> to enable command chaining</returns>
    public static StorageBuilder AddDiskBackend(this StorageBuilder builder, string name, Action<DiskProviderOptions> configure)
    {
        var settings = new DiskProviderOptions();
        configure(settings);
        return builder.AddProvider(name, new DiskProvider(settings));
    }
}