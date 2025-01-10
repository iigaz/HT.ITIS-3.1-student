using Dotnet.Homeworks.MainProject.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.MongoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoClient(this IServiceCollection services,
        MongoDbConfig mongoConfiguration)
    {
        services.AddSingleton(new MongoClient(mongoConfiguration.ConnectionString));
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        return services;
    }
}