using System.Linq.Expressions;
using System.Security.Authentication;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using BindingFlags = System.Reflection.BindingFlags;

namespace Dotnet.Homeworks.Infrastructure.Validation.Behaviors;

public class PermissionCheckBehaviour<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TResponse: Result
{
    private IPermissionCheck PermissionCheck { get; }
    
    public PermissionCheckBehaviour(IPermissionCheck permissionCheck)
    {
        PermissionCheck = permissionCheck;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var perm = await PermissionCheck.CheckPermissionAsync(request);
        var fail = perm.FirstOrDefault(result => result.IsFailure);
        if (fail != null)
            return BehavioursHelper.ToArbitraryResult<TResponse>(fail);
        return await next();
    }
}