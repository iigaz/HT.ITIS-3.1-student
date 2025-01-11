using Minio;
using MinioConfig = Dotnet.Homeworks.Storage.API.Configuration.MinioConfig;

namespace Dotnet.Homeworks.Storage.API.ServicesExtensions;

public static class AddMinioExtensions
{
    public static IServiceCollection AddMinioClient(this IServiceCollection services,
        MinioConfig minioConfiguration)
    {
        services.AddSingleton<IMinioClient>(_ => new MinioClient()
            .WithEndpoint(minioConfiguration.Endpoint, minioConfiguration.Port)
            .WithCredentials(minioConfiguration.Username, minioConfiguration.Password)
            .WithSSL(minioConfiguration.WithSsl)
            .Build());
        return services;
    }
}