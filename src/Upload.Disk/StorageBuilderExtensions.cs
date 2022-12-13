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
    public static StorageBuilder AddDiskProvider(this StorageBuilder builder, string name, Action<DiskProviderOptions> configure)
    {
        return AddDiskProvider(builder, name, (_, options) => configure(options));
    }
    
    /// <summary>
    /// Registers a Disk storage backend provider with given <paramref name="name"/>.
    /// </summary>
    /// <param name="builder">Storage configuration builder</param>
    /// <param name="name">Unique provider name</param>
    /// <param name="configure">Configuration delegate</param>
    /// <returns>Reference to <paramref name="builder"/> to enable command chaining</returns>
    public static StorageBuilder AddDiskProvider(this StorageBuilder builder, string name, Action<IServiceProvider, DiskProviderOptions> configure)
    {
        return builder.AddProvider(name, sp =>
        {
            var settings = new DiskProviderOptions();
            configure(sp, settings);
            return new DiskProvider(settings);
        });
    }
}