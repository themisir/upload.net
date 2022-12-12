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
    /// Combine object key with bucket name when generating browsable URLs using the <see cref="Browser"/>.
    /// </summary>
    public bool BrowseWithBucket { get; set; }
}