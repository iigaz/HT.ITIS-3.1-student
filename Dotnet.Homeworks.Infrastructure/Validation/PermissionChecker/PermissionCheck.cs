using System.Collections.Concurrent;
using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public class PermissionCheck : IPermissionCheck
{
    private ConcurrentBag<Type> RegisteredRequestTypes { get; } = new ConcurrentBag<Type>();

    public PermissionCheck(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    internal void AddRequestType(Type requestTypeType)
    {
        RegisteredRequestTypes.Add(requestTypeType);
    }

    private IServiceProvider ServiceProvider { get; }

    public async Task<IEnumerable<PermissionResult>> CheckPermissionAsync<TRequest>(TRequest request)
    {
        var ans = new List<PermissionResult>();
        await using (var scope = ServiceProvider.CreateAsyncScope())
        foreach (var type in RegisteredRequestTypes)
            if (typeof(TRequest).IsAssignableTo(type))
                foreach (var checker in scope.ServiceProvider.GetServices(typeof(IPermissionChecker<>).MakeGenericType(type)))
                    if (checker is IPermissionChecker<TRequest> permissionChecker)
                        ans.Add(await permissionChecker.CheckPermissionAsync(request));

        return ans.Where(result => result.IsFailure);
    }
}