using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Upload.Core;

namespace Upload.Disk;

public static class RouterExtensions
{
    public static IEndpointConventionBuilder MapUploadedStaticFiles(this IEndpointRouteBuilder endpoints,
        [StringSyntax("Route")] string routePrefix, string providerName)
    {
        var manager = endpoints.ServiceProvider.GetRequiredService<StorageManager>();
        if (manager.TryGetProvider(providerName, out var provider) &&
            provider is DiskProvider diskProvider)
        {
            return MapStaticFiles(endpoints, routePrefix, diskProvider.Options.Directory);
        }

        throw new ApplicationException($"There is no DiskProvider registered with name '{providerName}'");
    }

    public static IEndpointConventionBuilder MapStaticFiles(this IEndpointRouteBuilder endpoints,
        [StringSyntax("Route")] string routePrefix, string rootDirectory)
    {
        var pattern = Path.Join(routePrefix, "{**slug}");
        var handler = endpoints.CreateApplicationBuilder()
            .Use(IgnoreEndpoint)
            .UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(rootDirectory),
                RequestPath = routePrefix
            })
            .Build();

        return endpoints.Map(pattern, handler);
    }

    private static Task IgnoreEndpoint(HttpContext context, RequestDelegate next)
    {
        if (context.GetEndpoint() is { } prevEndpoint)
        {
            return IgnoreAsync(context, next, prevEndpoint);
        }

        return next(context);

        static async Task IgnoreAsync(HttpContext context, RequestDelegate next, Endpoint endpoint)
        {
            try
            {
                context.SetEndpoint(null);
                await next(context);
            }
            finally
            {
                context.SetEndpoint(endpoint);
            }
        } 
    }
}