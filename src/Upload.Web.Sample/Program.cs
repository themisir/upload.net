using Upload.Core;
using Upload.Disk;

var builder = WebApplication.CreateBuilder(args);

var tempDir = Directory.CreateTempSubdirectory().FullName;

builder.Services.AddUploadNet(x =>
{
    x.AddDiskBackend("disk", options =>
    {
        options.Directory = tempDir;
    });
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();