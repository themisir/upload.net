using Upload.Core;
using Upload.Core.Browser;
using Upload.Core.Web;
using Upload.Disk;

var builder = WebApplication.CreateBuilder(args);

var tempDir = Directory.CreateTempSubdirectory().FullName;

builder.Services.AddUploadNet()
    .AddDiskBackend("disk", options =>
    {
        options.Browser = new DefaultStorageBrowser("http://localhost:5000/files/{key}");
        options.Directory = tempDir;
    });

var app = builder.Build();

app.UseStaticFiles();

app.MapUploadManyFiles("/uploads/disk", "disk");
app.MapUploadedStaticFiles("/files", "disk");

app.MapFallbackToFile("index.html");

app.Run();