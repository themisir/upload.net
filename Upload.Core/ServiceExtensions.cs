using Microsoft.Extensions.DependencyInjection;
using Upload.Core.Browser;

namespace Upload.Core;

public static class ServiceExtensions
{
    public static IServiceCollection AddDefaultStorageBrowser(this IServiceCollection services, Action<DefaultStorageBrowserSettings> configure)
    {
        services.Configure(configure);
        
        services.AddSingleton<IStorageBrowser, DefaultStorageBrowser>();
        
        return services;
    }
}