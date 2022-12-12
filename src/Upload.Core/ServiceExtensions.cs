using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Upload.Core.Browser;
using Upload.Core.Service;

namespace Upload.Core;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds <see cref="StorageManager"/> service to the service collection.
    /// </summary>
    /// <param name="services">The service collection instance</param>
    /// <returns>A builder to configure <see cref="StorageManager"/> in various ways</returns>
    public static StorageBuilder AddUploadNet(this IServiceCollection services)
    {
        services.Configure<StorageManagerOptions>(_ => { });
        services.TryAddSingleton<StorageManager>();
        
        return new StorageBuilder(services);
    }
}