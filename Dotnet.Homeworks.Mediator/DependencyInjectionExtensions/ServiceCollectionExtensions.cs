using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
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
            {
                Console.WriteLine($"Registered: {inf.ToString()}");
                services.AddScoped(inf, type);
                if (inf.GenericTypeArguments[0].IsClass)
                {
                    foreach (var inf2 in inf.GenericTypeArguments[0].GetInterfaces())
                    {
                        if (inf2.IsGenericType && inf2.GetGenericTypeDefinition().IsAssignableTo(typeof(IRequest<>))
                            || !inf2.IsGenericType && inf2.IsAssignableTo(typeof(IRequest)))
                        {
                            var infCorrected = inf.GenericTypeArguments.Length == 1
                                ? inf.GetGenericTypeDefinition().MakeGenericType(inf2)
                                : inf.GetGenericTypeDefinition().MakeGenericType(inf2, inf.GenericTypeArguments[1]);
                            Console.WriteLine($"Registered: {infCorrected}");
                            services.AddScoped(infCorrected, type);
                        }
                    }
                }
            }
        return services;
    }
}