using Upload.Core;
using Upload.Core.Browser;
using Upload.Disk;

var builder = WebApplication.CreateBuilder(args);

var tempDir = Directory.CreateTempSubdirectory().FullName;

builder.Services.AddUploadNet()
    .AddDiskBackend("disk", options =>
    {
        options.Browser = new DefaultStorageBrowser("http://localhost:8080/files/{key}");
        options.Directory = tempDir;
    });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapFallbackToFile("/files", tempDir);

app.Run();