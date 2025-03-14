using Upload.AwsS3;
using Upload.Core;
using Upload.Core.Browser;
using Upload.Core.Web;
using Upload.Disk;

var builder = WebApplication.CreateBuilder(args);

var tempDir = Directory.CreateTempSubdirectory().FullName;

builder.Services.AddAmazonS3(builder.Configuration.GetSection("AmazonS3"));

builder.Services.AddUploadNet()
    .AddAwsS3Provider("shared", options =>
    {
        options.BucketName = builder.Configuration["UploadNet:Shared:BucketName"]!;
        options.Browser = new DefaultStorageBrowser(builder.Configuration["UploadNet:Shared:UrlFormat"]!);
    })
    .AddDiskProvider("local", options =>
    {
        options.Browser = new DefaultStorageBrowser(builder.Configuration["UploadNet:Local:UrlFormat"]!);
        options.Directory = tempDir;
    });

var app = builder.Build();

app.UseStaticFiles();

app.MapUploadManyFiles("/uploads/local", "local", allowSubDirectories: false);
app.MapUploadManyFiles("/uploads/local-with-sub-dirs", "local", allowSubDirectories: true);
app.MapUploadManyFiles("/uploads/shared", "shared");

app.MapUploadedStaticFiles("/files", "local");

app.MapFallbackToFile("index.html");

app.Run();
