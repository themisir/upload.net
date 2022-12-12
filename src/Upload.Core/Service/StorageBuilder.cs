using Microsoft.Extensions.DependencyInjection;

namespace Upload.Core.Service;

public sealed class StorageBuilder
{
    public readonly IServiceCollection Services;

    public StorageBuilder(IServiceCollection services)
    {
        Services = services;
    }

    /// <summary>
    /// Register a storage provider with given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Unique provider name</param>
    /// <param name="factory">An factory to instantiate a provider instance</param>
    /// <returns>Self reference for chaining commands</returns>
    public StorageBuilder AddProvider(string name, Func<IServiceProvider, IStorageProvider> factory)
    {
        Services.Configure<StorageManagerOptions>(options =>
        {
            options.AddProvider(name, factory);
        });

        return this;
    }
}