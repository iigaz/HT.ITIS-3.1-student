using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class CqrsDecorator<TRequest, TResponse> : ValidationDecorator<TRequest, TResponse>
{
    protected CqrsDecorator(IPermissionCheck permissionCheck, IValidator<TRequest>? validator) : base(permissionCheck, validator)
    {
    }
}