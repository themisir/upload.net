using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Upload.Core.Extra;

namespace Upload.Core.Web;

internal sealed class UploadHandler
{
    private readonly StorageManager _storageManager;
    private readonly string _providerName;

    private readonly record struct UploadedFileDto(string Name, string? Url);

    public UploadHandler(IServiceProvider serviceProvider, string providerName)
    {
        _providerName = providerName;
        _storageManager = serviceProvider.GetRequiredService<StorageManager>();
    }

    public async Task UploadManyFiles(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        var files = form.Files.GetFiles("files");

        var result = new List<UploadedFileDto>(files.Count);

        foreach (var file in files)
        {
            var key = KeyUtils.GetUniqueKey(file.FileName);
            
            await using var source = file.OpenReadStream();
            var fileRef = await _storageManager.CreateFile(_providerName, key, source);
            
            result.Add(new UploadedFileDto(fileRef.Key, fileRef.Url));
        }

        await context.Response.WriteAsJsonAsync(result);
    }
}