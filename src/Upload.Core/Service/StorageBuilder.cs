using Microsoft.Extensions.DependencyInjection;

namespace Upload.Core.Service;

public sealed class StorageBuilder
{
    public readonly IServiceCollection Services;

    public StorageBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public StorageBuilder AddBackend(string name, IStorageProvider provider)
    {
        Services.Configure<StorageManagerOptions>(options =>
        {
            options.AddProvider(name, provider);
        });

        return this;
    }
}