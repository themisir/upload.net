using Microsoft.Extensions.DependencyInjection;
using Upload.Core;

namespace Upload.Disk;

public static class ServiceExtensions
{
    public static IServiceCollection AddDiskBackend(this IServiceCollection services, Action<DiskBackendSettings> configure)
    {
        services.Configure(configure);
        
        services.AddSingleton<IStorageBackend, DiskBackend>();
        
        return services;
    }
}