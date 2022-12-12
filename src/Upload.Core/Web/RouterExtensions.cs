using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Upload.Core.Web;

public static class RouterExtensions
{
    public static IEndpointConventionBuilder MapUploadManyFiles(this IEndpointRouteBuilder builder, string pattern, string providerName)
    {
        var handler = new UploadHandler(builder.ServiceProvider, providerName);
        return builder.MapPost(pattern, handler.UploadManyFiles);
    }
}