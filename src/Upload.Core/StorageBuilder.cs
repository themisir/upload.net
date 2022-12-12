using Microsoft.Extensions.DependencyInjection;
using Upload.Core.Service;

namespace Upload.Core;

public sealed class StorageBuilder
{
    internal readonly IServiceCollection Services;
    internal readonly Dictionary<string, IStorageProvider> Providers = new();

    public StorageBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public StorageBuilder AddBackend(string name, IStorageProvider providerImpl)
    {
        if (!Providers.TryAdd(name, providerImpl))
        {
            throw new ApplicationException($"A storage backend with name '{name}' is already configured");
        }
        
        return this;
    }
}