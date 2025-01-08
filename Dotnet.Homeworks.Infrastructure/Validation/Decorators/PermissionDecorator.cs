using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class PermissionDecorator<TRequest, TResponse>: IDecorator<TRequest, TResponse>
{
    private IPermissionCheck PermissionCheck { get; }
    
    protected PermissionDecorator(IPermissionCheck permissionCheck) : base()
    {
        PermissionCheck = permissionCheck;
    }

    public virtual async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var results = await PermissionCheck.CheckPermissionAsync(request);
        var result = results.FirstOrDefault(res => res.IsFailure) ?? new PermissionResult(true);
        
        return new Result<TResponse>(default, result.IsSuccess, result.Error);
    }
}