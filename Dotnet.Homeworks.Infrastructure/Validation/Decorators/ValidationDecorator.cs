using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;
using FluentValidation.Results;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class ValidationDecorator<TRequest, TResponse> : PermissionDecorator<TRequest, TResponse>
{
    private IValidator<TRequest>? Validator { get; }
    protected ValidationDecorator(IPermissionCheck permissionCheck, IValidator<TRequest>? validator) : base(permissionCheck)
    {
        Validator = validator;
    }

    public override async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var decResult = await base.Handle(request, cancellationToken);
        if (decResult.IsFailure)
            return decResult;
        var result = await (Validator?.ValidateAsync(request, cancellationToken) ??
                            Task.FromResult(new ValidationResult()));
        if (result.IsValid)
            return new Result<TResponse>(default, true);
        return new Result<TResponse>(default, false, result.Errors.FirstOrDefault()?.ErrorMessage);
    }
}