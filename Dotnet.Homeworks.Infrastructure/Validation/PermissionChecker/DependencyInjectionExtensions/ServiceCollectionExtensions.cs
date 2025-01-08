using System.Reflection;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    public static void AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly assembly
    )
    {
        serviceCollection.AddPermissionChecks(new[] { assembly });
    }

    public static void AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly[] assemblies
    )
    {
        var set = new HashSet<Type>();
        foreach (var assembly in assemblies)
        foreach (var type in assembly.GetTypes().Where(t => t.IsClass))
        foreach (var inf in type.GetInterfaces())
            if (inf.IsGenericType && inf.GetGenericTypeDefinition().IsAssignableTo(typeof(IPermissionChecker<>)))
            {
                serviceCollection.AddScoped(inf, type);
                set.Add(inf.GenericTypeArguments[0]);
            }

        serviceCollection.AddSingleton<IPermissionCheck, PermissionCheck>(provider =>
        {
            var check = new PermissionCheck(provider);
            foreach (var type in set)
                check.AddRequestType(type);
            return check;
        });
    }
}