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
    /// <param name="provider">An instance of the provider</param>
    /// <returns>Self reference for chaining commands</returns>
    public StorageBuilder AddProvider(string name, IStorageProvider provider)
    {
        Services.Configure<StorageManagerOptions>(options =>
        {
            options.AddProvider(name, provider);
        });

        return this;
    }
}