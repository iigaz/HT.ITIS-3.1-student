using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] handlersAssemblies)
    {
        services.AddSingleton<IMediator, ServiceMediator>();
        foreach (var assembly in handlersAssemblies)
        foreach (var type in assembly.GetTypes().Where(t => t.IsClass))
        foreach (var inf in type.GetInterfaces())
            if (inf.IsGenericType && (inf.GetGenericTypeDefinition().IsAssignableTo(typeof(IRequestHandler<>)) ||
                                      inf.GetGenericTypeDefinition().IsAssignableTo(typeof(IRequestHandler<,>))))
                services.AddScoped(inf, type);
        return services;
    }
}