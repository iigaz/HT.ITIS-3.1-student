using System.Reflection;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    public static void AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly assembly
    )
    {
        // ВОТ НЕ МОГЛИ СРАЗУ СКАЗАТЬ, ЧТО НУЖНО НОВЫЕ ИНТЕРФЕЙСЫ ДЛЯ PERMISSIONCHECK СОЗДАВАТЬ
        // ЗАДАНИЕ ТО ВЫПОЛНЯЕТСЯ БЕЗ НОВЫХ ИНТЕРФЕЙСОВ
        // И МНЕ ВОТ ТЕПЕРЬ СПУСТЯ НЕСКОЛЬКО ЧАСОВ ОБДУМЫВАНИЯ ЧТО ОТ МЕНЯ ХОТЯТ ПЕРЕПИСЫВАТЬ ЛОГИКУ PERMISSIONCHECK,
        // КОГДА ОНА БЫЛА ПОЛНОСТЬЮ РЕАЛИЗОВАНА.
        throw new NotImplementedException();
    }
    
    public static void AddPermissionChecks(
        this IServiceCollection serviceCollection,
        Assembly[] assemblies
    )
    {
        throw new NotImplementedException();
    }
}