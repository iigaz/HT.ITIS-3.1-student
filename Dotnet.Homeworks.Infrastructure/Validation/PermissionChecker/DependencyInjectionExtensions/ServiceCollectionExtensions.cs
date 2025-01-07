using System.Reflection;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    public static void AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly assembly
    )
    {
        serviceCollection.AddPermissionChecks(new []{ assembly });
    }
    
    public static void AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly[] assemblies
    )
    {
        serviceCollection.AddSingleton<IPermissionCheck, PermissionCheck>(provider =>
        {
            var check = new PermissionCheck(provider);
            foreach (var assembly in assemblies)
            foreach (var type in assembly.GetTypes().Where(t => t.IsClass))
            foreach (var inf in type.GetInterfaces())
                if (inf.IsGenericType && inf.GetGenericTypeDefinition().IsAssignableTo(typeof(IPermissionChecker<>)))
                {
                    serviceCollection.AddScoped(inf, type);
                    check.AddRequestType(inf.GenericTypeArguments[0]);
                }

            return check;
        });
    }
}