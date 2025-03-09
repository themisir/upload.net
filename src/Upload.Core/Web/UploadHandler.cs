using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Upload.Core.Extra;

namespace Upload.Core.Web;

internal sealed class UploadHandler
{
    private readonly StorageManager _storageManager;
    private readonly string _providerName;
    private readonly bool _allowSubDirectories;

    private readonly record struct UploadedFileDto(string Name, string? Url);

    public UploadHandler(IServiceProvider serviceProvider,
        string providerName,
        bool allowSubDirectories)
    {
        _providerName = providerName;
        _allowSubDirectories = allowSubDirectories;
        _storageManager = serviceProvider.GetRequiredService<StorageManager>();
    }

    public async Task UploadManyFiles(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        var files = form.Files.GetFiles("files");

        var result = new List<UploadedFileDto>(files.Count);

        if (context.Request.Query.TryGetValue("dir", out var dirParam) && !_allowSubDirectories)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Subdirectories are not allowed");
            return;
        }

        var uploadPath = string.IsNullOrWhiteSpace(dirParam.ToString())
            ? null
            : dirParam.ToString().TrimStart(Path.DirectorySeparatorChar); // Don't allow to upload to root directory

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
