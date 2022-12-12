using Microsoft.Extensions.DependencyInjection;
using Upload.Core.Browser;
using Upload.Core.Service;

namespace Upload.Core;

public static class ServiceExtensions
{
    public static StorageBuilder AddUploadNet(this IServiceCollection services)
    {
        services.Configure<StorageManagerOptions>(_ => { });
        services.AddSingleton<StorageManager>();
        
        return new StorageBuilder(services);
    }
}