using Microsoft.Extensions.DependencyInjection;
using Upload.Core.Browser;
using Upload.Core.Service;

namespace Upload.Core;

public static class ServiceExtensions
{
    public static IServiceCollection AddUploadNet(this IServiceCollection services, Action<StorageBuilder> configure)
    {
        var builder = new StorageBuilder(services);
        configure(builder);
        services.AddSingleton(new StorageManager(builder));
        return services;
    }
}