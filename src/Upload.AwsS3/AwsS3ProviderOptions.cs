using Amazon.S3;
using Upload.Core.Browser;
using Upload.Core.Service;

namespace Upload.AwsS3;

public sealed class AwsS3ProviderOptions
{
    /// <summary>
    /// Gets or sets S3 storage bucket name.
    /// </summary>
    public string BucketName { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets storage browser implementation that'll be used for creating object URLs.
    /// </summary>
    /// <seealso cref="DefaultStorageBrowser"/>
    public IStorageBrowser? Browser { get; set; }
    
    /// <summary>
    /// Gets or sets whether or not to combine object key with bucket name when generating browsable URLs using
    /// the <see cref="Browser"/>.
    /// </summary>
    public bool BrowseWithBucket { get; set; }
    
    /// <summary>
    /// Gets or sets custom S3 client instance to be used with this provider. If not set the <see cref="IAmazonS3"/>
    /// instance from the Dependency Injection container will be used instead.
    /// </summary>
    public IAmazonS3? AmazonS3 { get; set; }
}