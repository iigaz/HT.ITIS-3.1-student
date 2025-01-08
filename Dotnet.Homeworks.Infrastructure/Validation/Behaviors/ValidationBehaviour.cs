using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Infrastructure.Validation.Behaviors;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse: Result
{
    private IValidator<TRequest>? Validator { get; }
    
    public ValidationBehaviour(IValidator<TRequest>? validator=null)
    {
        Validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (Validator != null)
        {
            var result = await Validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
                return BehavioursHelper.ToArbitraryResult<TResponse>(result);
        }
        return await next();
    }
}