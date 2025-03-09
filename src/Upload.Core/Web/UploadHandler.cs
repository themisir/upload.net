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

        var uploadPathParam = context.Request.Query["dir"].ToString();
        var uploadPath = string.IsNullOrWhiteSpace(uploadPathParam)
            ? null
            : uploadPathParam.TrimStart(Path.DirectorySeparatorChar); // Don't allow to upload to root directory

        foreach (var file in files)
        {
            var key = KeyUtils.GetUniqueKey(file.FileName);
            var fullKey = uploadPath is null
                ? key : Path.Combine(uploadPath, key);

            KeyUtils.MushBeSafeKey(fullKey);

            await using var source = file.OpenReadStream();
            var fileRef = await _storageManager.CreateFile(_providerName, fullKey, source);

            result.Add(new UploadedFileDto(fileRef.Key, fileRef.Url));
        }

        await context.Response.WriteAsJsonAsync(result);
    }
}
