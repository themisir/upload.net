using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using Upload.Core.Service;

namespace Upload.AwsS3;

public static class StorageBuilderExtensions
{
    /// <summary>
    /// Registers an AWS S3 storage backend provider with given <paramref name="name"/>.
    /// </summary>
    /// <param name="builder">Storage configuration builder</param>
    /// <param name="name">Unique provider name</param>
    /// <param name="configure">Configuration delegate</param>
    /// <returns>Reference to <paramref name="builder"/> to enable command chaining</returns>
    public static StorageBuilder AddAwsS3(this StorageBuilder builder, string name, Action<AwsS3ProviderOptions> configure)
    {
        return AddAwsS3(builder, name, (_, options) => configure(options));
    }
    
    /// <summary>
    /// Registers an AWS S3 storage backend provider with given <paramref name="name"/>.
    /// </summary>
    /// <param name="builder">Storage configuration builder</param>
    /// <param name="name">Unique provider name</param>
    /// <param name="configure">Configuration delegate</param>
    /// <returns>Reference to <paramref name="builder"/> to enable command chaining</returns>
    public static StorageBuilder AddAwsS3(this StorageBuilder builder, string name, Action<IServiceProvider, AwsS3ProviderOptions> configure)
    {
        return builder.AddProvider(name, sp =>
        {
            var options = new AwsS3ProviderOptions();
            configure(sp, options);
            return new AwsS3Provider(sp.GetRequiredService<IAmazonS3>(), options);
        });
    }
}