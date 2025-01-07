using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] handlersAssemblies)
    {
        foreach (var assembly in handlersAssemblies)
        foreach (var type in assembly.GetTypes().Where(t => t.IsClass))
        foreach (var inf in type.GetInterfaces())
            if (inf.IsGenericType && (inf.GetGenericTypeDefinition().IsAssignableTo(typeof(IRequestHandler<>)) ||
                                      inf.GetGenericTypeDefinition().IsAssignableTo(typeof(IRequestHandler<,>))))
            {
                Console.WriteLine($"Registered: {inf}");
                services.AddScoped(inf, type);
            }
        services.AddSingleton<IMediator, ServiceMediator>(provider =>
        {
            var med = new ServiceMediator(provider);
            foreach (var assembly in handlersAssemblies)
                    foreach (var type in assembly.GetTypes().Where(t => t.IsClass))
                    foreach (var inf in type.GetInterfaces())
                        if (inf.IsGenericType && (inf.GetGenericTypeDefinition().IsAssignableTo(typeof(IRequestHandler<>)) ||
                                                  inf.GetGenericTypeDefinition().IsAssignableTo(typeof(IRequestHandler<,>))))
                        {
                            med.AddRequestImpl(inf.GenericTypeArguments[0]);
                        }

            return med;
        });
        
        return services;
    }
}