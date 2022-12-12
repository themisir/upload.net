using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Upload.Core.Extra;

namespace Upload.Core;

internal sealed class UploadHandler
{
    private readonly IStorageBackend _storageBackend;
    private readonly IStorageBrowser _storageBrowser;

    private readonly record struct UploadedFileDto(string Name, string Url);

    public UploadHandler(IServiceProvider serviceProvider)
    {
        _storageBackend = serviceProvider.GetRequiredService<IStorageBackend>();
        _storageBrowser = serviceProvider.GetRequiredService<IStorageBrowser>();
    }

    public async Task Upload(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        var files = form.Files.GetFiles("files");

        var result = new List<UploadedFileDto>(files.Count);

        foreach (var file in files)
        {
            var key = KeyUtils.GetUniqueKey(file.Name);
            
            await using var source = file.OpenReadStream();
            
            var fileRef = await _storageBackend.CreateFile("bucket", key, source);
            var fileUrl = await _storageBrowser.GetPublicUrl(fileRef);
            
            result.Add(new UploadedFileDto(fileRef.Key, fileUrl));
        }

        await context.Response.WriteAsJsonAsync(result);
    }
}