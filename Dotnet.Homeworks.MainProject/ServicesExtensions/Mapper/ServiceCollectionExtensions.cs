using System.Reflection;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Mapper;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services, Assembly mapperConfigsAssembly)
    {
        foreach (var type in mapperConfigsAssembly.GetTypes().Where(t => t.IsClass))
        foreach (var inf in type.GetInterfaces())
            if (inf.Name.EndsWith("Mapper"))
                services.AddSingleton(inf, type);

        return services;
    }
}