using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Upload.AwsS3;

public static class ServiceExtensions
{
    public static IServiceCollection AddAmazonS3(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetAWSOptions(null);
        if (configuration.GetSection("Credentials") is { } credSection &&
            credSection.GetValue<string?>("Type") is {} credType)
        {
            options.Credentials = credType switch
            {
                "Basic" => new BasicAWSCredentials(credSection["AccessKey"], credSection["SecretKey"]),
                "Anonymous" => new AnonymousAWSCredentials(),
                "EnvironmentVariables" => new EnvironmentVariablesAWSCredentials(),
                _ => options.Credentials
            };

        }

        services.AddAWSService<IAmazonS3>(options);
        return services;
    }

    public static IServiceCollection AddAmazonS3(this IServiceCollection services, Action<AWSOptions> configure)
    {
        var options = new AWSOptions();
        configure(options);
        services.AddAWSService<IAmazonS3>(options);
        return services;
    }
}