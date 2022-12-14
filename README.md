# Upload.NET

This project aims to be all in one solution to manage file upload workflows on your .NET projects. It will probably not be the best one but I'm hopeful that at least it will reduce my need to re-implement another upload system for each project. At it's core Upload.NET tries to be a simple way to add file upload feature to content management systems.

Take a sneak peek below to see what it feels like to work use `Upload.NET` APIs.

```csharp
// save file on a provider called 'public'
await _storageManager.CreateFile("public", "users/1553/profile.png", inputStream);

// retrieve file reference to read or delete it
if (await _storageManager.GetFile("public", "users/1553/profile.png") is { } fileRef) {
    Console.WriteLine(fileRef.Key);
}
```

## Getting Started

Packages are available to download from NuGet, choose and download at least one of the provider packages.

- [Upload.Core](https://www.nuget.org/packages/Upload.Core/) - Core abstraction, included as part of the other provider packages
- [Upload.Disk](https://www.nuget.org/packages/Upload.Disk/) - File system based implementation of the storage system
- [Upload.AwsS3](https://www.nuget.org/packages/Upload.AwsS3/) - S3 compatible system implemented using AWS S3 SDK

You can add upload net services to a service collection using `AddUploadNet` extension method. It will return a builder that you can use to register various storage providers.

```csharp
builder.Services.AddUploadNet()
    .AddDiskProvider("primary", options =>
    {
        options.Browser = new DefaultStorageBrowser("http://localhost:5000/download/{key}");
        options.Directory = "/data";
    });
    
var app = builder.Build();
    
app.MapUploadManyFiles("/upload", "primary");
app.MapUploadedStaticFile("/download", "primary");
```

#### Next up:

Check out [endpoints](#endpoints) section to find out about useful endpoints to quickly add API routes for doing stuff like uploading files or serving the uploaded files.

## Core concepts

Upload.NET has some core concepts that explaining them beforehand will be helpful to dig deeper going forward.

### `StorageManager`

A singleton interface that lets you work with multiple providers at once. It creates and manages all the registered storage providers. It exposes the same method set that `IStorageProvider` does, with an exception that it requires an additional `providerName` parameter to specify which provider needs to be used to handle a specific request.

### Object key

Each uploaded file needs to be represented with an unique provider specific key. It has similar characteristics to a regular file names or S3 object keys.

### Provider

Upload.NET at its core does support working with various providers at the same time. To hide the provider details from the implementation itself Storage.NET uses application wide unique names to specify providers.

It's useful to think providers as a storage targets rather than a specific implementation. For example you can register a provider with a name `PublicFiles` and use disk provider on development environments and S3 provider on production environments to simplify development workflow while having flexibility on production.

To achieve that all you'll need is to just conditionally register a different storage provider based on the current environment.

```csharp
if (Environment.IsDevelopment())
{
    services.AddUploadNet()
        .AddDiskProvider("PublicImages", options =>
        {
            options.Directory = "/tmp/files";
        });
}
else
{
    services.AddUploadNet()
        .AddAwsS3Provider("PublicImages", options =>
        {
            options.BucketName = "public-images";
        });
}
```

You can register as many providers as you want, the only thing you will need to look after is to make sure providers names are unique.

```csharp
services.AddUploadNet()
    .AddDiskProvider("CacheFiles", options => {  })
    .AddDiskProvider("Local", options => {  })
    .AddAwsS3Provder("CDN", options => {  }); 
```

### `IStorageProvider`

A storage provider is an implementation that provides set of tools to work with files in a specific storage backend. Currently there are **Upload.Disk** and **Upload.AwsS3** implementations available as separate NuGet packages.

### `IFileRef`

File references lets you to work with uploaded files on various storage providers. Right now it contains methods for retrieving the file key, creating a `Stream` to read its contents and a way to delete files. Additionally it does also contain an optional `Url` property which is usually used for publicly accessing to the file, more on that is written below on the `IStorageBrowser` section.

Please note that file reference implementations are provider specific. Providers might behave differently for different operations, but a few suggested conventions to look after are:

- Open only one reader stream per `IFileRef` object.
- Dispose the reader stream after consuming. You can use `using` statement for that.
- After the reader stream has been disposed consider fetching another `IFileRef` from the storage manager if you want to read it again.
- Do not try to open a reader stream after calling `IFileRef.Delete`

### `IStorageBrowser`

First of all if the name is confusing I am sorry for that but I couldn't come up with a better name for an interface that would be used for converting file keys to browsable URLs. Storage browsers are usually consumed by storage providers to fill up a value for `IFileRef.Url` property. The exact way to set provider's browser does depend on each providers implementation, but the convention is to consume `Browser` as an options property during the configuration.

The core package does include `DefaultStorageBrowser` implementation which is a simplest implementation that can be suitable for various use cases involving public files. To use it all you need to have is a URL template - A partial URL with a `{key}` template parameter that will be replaced with the file key. A sample URL template does look like that.

```plain
https://s3.example.org/files/{key}
```

## Endpoints

Aside from the above abstractions, Upload.NET does also provide various endpoint builder extensions to simplify creation of various API endpoints to manipulate storage system.

### `MapUploadManyFiles`

This endpoint creates an API route for uploading multiple files using `multipart/form-data` encoding. The method does accepts 2 parameters: a route pattern and provider name to use for saving the uploaded files. After a successful upload the endpoint responds with a JSON payload containing array of objects containing file name and urls.

Library: `Upload.Core`

### `MapStaticFiles`

An endpoint builder implementation of standard ASP.NET Core `UseStaticFiles` middleware. The method does accepts 2 parameter: url prefix and absolute path to the root directory containing the files.

Library: `Upload.Disk`

### `MapUploadedStaticFiles`

This method adds a route similar to `MapUploadManyFiles` but, instead of requiring root directory it expects provider name which then it'll look up for the provider to get `Directory` option from if the provider is a `DiskProvider`.

The method will throw an exception if a provider is not registered with given name or the provider type is not `DiskProvider`. It is a good idea to check for the provider type if you conditionally switch provider type during the configuration phrase. For example if you are only using disk provider on the development environments, wrap `MapUploadedStaticFiles` call with an if statement checking for the development environment.

Library: `Upload.Disk`

## License

The project is licensed under Apache-2.0 license terms.

## Questions

If you have any questions or suggestions, please file an issue with sufficient details explaining your request / bug report.
