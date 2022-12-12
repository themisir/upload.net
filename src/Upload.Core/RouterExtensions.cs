using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Upload.Core;

public static class RouterExtensions
{
    public static IEndpointConventionBuilder MapUploadEndpoint(this IEndpointRouteBuilder builder, string pattern = "/upload")
    {
        var handler = new UploadHandler(builder.ServiceProvider);

        return builder.MapPost(pattern, handler.Upload);
    }
}