using Dotnet.Homeworks.MainProject.Configuration;
using MongoDB.Driver;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.MongoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoClient(this IServiceCollection services,
        MongoDbConfig mongoConfiguration)
    {
        services.AddSingleton(new MongoClient(mongoConfiguration.ConnectionString));
        return services;
    }
}